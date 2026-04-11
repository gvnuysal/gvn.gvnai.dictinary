import { Component, ChangeDetectionStrategy, signal, computed, inject, OnDestroy } from '@angular/core';
import { DecimalPipe, UpperCasePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { QuizService } from '../../../../services/quiz.service';
import { WordService } from '../../../../services/word.service';
import { QuizQuestionDto, QuizAnswerResultDto, QuizResultDto } from '../../../../models/quiz.model';
import { WordDto, SenseDto } from '../../../../models/word.model';
import { TtsService } from '../../../../services/tts.service';
import { ProfileService } from '../../../../services/profile.service';

@Component({
  selector: 'dict-quiz',
  standalone: true,
  imports: [DecimalPipe, UpperCasePipe, RouterLink],
  templateUrl: './quiz.component.html',
  styleUrl: './quiz.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class QuizComponent implements OnDestroy {
  private readonly quizService = inject(QuizService);
  private readonly wordService = inject(WordService);
  private readonly profileService = inject(ProfileService);
  readonly tts = inject(TtsService);
  private quizAutoSpeak = true;

  readonly gameState = signal<'idle' | 'playing' | 'answered' | 'finished'>('idle');
  readonly sessionId = signal<string | null>(null);
  readonly question = signal<QuizQuestionDto | null>(null);
  readonly answerResult = signal<QuizAnswerResultDto | null>(null);
  readonly quizResult = signal<QuizResultDto | null>(null);
  readonly countdown = signal(10);
  readonly totalScore = signal(0);
  readonly questionNumber = signal(0);
  readonly correctCount = signal(0);
  readonly wrongCount = signal(0);
  readonly selectedOptionId = signal<string | null>(null);
  readonly loading = signal(false);

  // Word card modal
  readonly showWordModal = signal(false);
  readonly modalWord = signal<WordDto | null>(null);
  readonly modalLoading = signal(false);

  private timerInterval: ReturnType<typeof setInterval> | null = null;
  private questionStartTime = 0;

  readonly timerOffset = computed(() => {
    const circumference = 2 * Math.PI * 45;
    const progress = this.countdown() / 10;
    return circumference * (1 - progress);
  });

  ngOnDestroy(): void {
    this.clearTimer();
  }

  startGame(): void {
    this.loading.set(true);
    // Profil ayarından auto-speak tercihini oku
    this.profileService.getProfile().subscribe({
      next: (p) => this.quizAutoSpeak = p.apiSettings.quizAutoSpeak,
    });
    this.quizService.startQuiz().subscribe({
      next: (sessionId) => {
        this.sessionId.set(sessionId);
        this.totalScore.set(0);
        this.questionNumber.set(0);
        this.correctCount.set(0);
        this.wrongCount.set(0);
        this.quizResult.set(null);
        this.loading.set(false);
        this.loadNextQuestion();
      },
      error: () => this.loading.set(false),
    });
  }

  loadNextQuestion(): void {
    const sid = this.sessionId();
    if (!sid) return;

    this.showWordModal.set(false);
    this.modalWord.set(null);
    this.loading.set(true);
    this.quizService.getNextQuestion(sid).subscribe({
      next: (q) => {
        this.question.set(q);
        this.questionNumber.set(q.questionNumber);
        this.totalScore.set(q.totalScore);
        this.correctCount.set(q.correctCount);
        this.wrongCount.set(q.wrongCount);
        this.selectedOptionId.set(null);
        this.answerResult.set(null);
        this.gameState.set('playing');
        this.loading.set(false);
        this.startTimer();
        // Otomatik seslendir
        if (this.quizAutoSpeak && this.tts.isSupported) {
          this.tts.speak(q.lemma, 'en-US');
        }
      },
      error: () => {
        this.loading.set(false);
        this.finishGame();
      },
    });
  }

  selectOption(optionId: string): void {
    if (this.gameState() !== 'playing') return;
    this.clearTimer();
    this.selectedOptionId.set(optionId);
    this.submitAnswer(optionId, Date.now() - this.questionStartTime);
  }

  onTimeout(): void {
    this.clearTimer();
    this.selectedOptionId.set(null);
    this.submitAnswer(null, 10000);
  }

  sortedSenses(senses: SenseDto[]): SenseDto[] {
    return [...senses].sort((a, b) => a.senseNumber - b.senseNumber);
  }

  // Doğru cevap modal'ından devam
  continueFromModal(): void {
    this.showWordModal.set(false);
    this.modalWord.set(null);
    this.loadNextQuestion();
  }

  finishGame(): void {
    this.clearTimer();
    this.showWordModal.set(false);
    const sid = this.sessionId();
    if (!sid) return;

    this.loading.set(true);
    this.quizService.completeQuiz(sid).subscribe({
      next: (result) => {
        this.quizResult.set(result);
        this.gameState.set('finished');
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
        this.gameState.set('idle');
      },
    });
  }

  private submitAnswer(optionId: string | null, responseTimeMs: number): void {
    const sid = this.sessionId();
    const q = this.question();
    if (!sid || !q) return;

    this.loading.set(true);
    this.quizService.submitAnswer(sid, {
      wordId: q.wordId,
      selectedOptionId: optionId,
      correctOptionId: q.correctOptionId,
      responseTimeMs,
    }).subscribe({
      next: (result) => {
        this.answerResult.set(result);
        this.totalScore.set(result.totalScore);
        if (result.isCorrect) {
          this.correctCount.update(c => c + 1);
        } else {
          this.wrongCount.update(c => c + 1);
        }
        this.gameState.set('answered');
        this.loading.set(false);

        if (result.isCorrect) {
          // Doğru cevap → kelime kartını yükle ve modal göster
          this.modalLoading.set(true);
          this.wordService.getWordById(q.wordId).subscribe({
            next: (word) => {
              this.modalWord.set(word);
              this.modalLoading.set(false);
              this.showWordModal.set(true);
            },
            error: () => {
              this.modalLoading.set(false);
              // Yüklenemezse 2sn sonra devam et
              setTimeout(() => this.loadNextQuestion(), 2000);
            },
          });
        } else {
          // Yanlış cevap → 2sn sonra otomatik devam
          setTimeout(() => this.loadNextQuestion(), 2000);
        }
      },
      error: () => this.loading.set(false),
    });
  }

  private startTimer(): void {
    this.clearTimer();
    this.countdown.set(10);
    this.questionStartTime = Date.now();
    this.timerInterval = setInterval(() => {
      const remaining = this.countdown() - 1;
      if (remaining <= 0) {
        this.countdown.set(0);
        this.onTimeout();
      } else {
        this.countdown.set(remaining);
      }
    }, 1000);
  }

  private clearTimer(): void {
    if (this.timerInterval) {
      clearInterval(this.timerInterval);
      this.timerInterval = null;
    }
  }
}
