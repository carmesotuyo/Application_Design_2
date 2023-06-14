import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OffensiveWordsComponent } from './offensive-words.component';

describe('OffensiveWordsComponent', () => {
  let component: OffensiveWordsComponent;
  let fixture: ComponentFixture<OffensiveWordsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OffensiveWordsComponent]
    });
    fixture = TestBed.createComponent(OffensiveWordsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
