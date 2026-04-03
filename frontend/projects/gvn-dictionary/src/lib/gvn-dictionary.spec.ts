import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GvnDictionary } from './gvn-dictionary';

describe('GvnDictionary', () => {
  let component: GvnDictionary;
  let fixture: ComponentFixture<GvnDictionary>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GvnDictionary],
    }).compileComponents();

    fixture = TestBed.createComponent(GvnDictionary);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
