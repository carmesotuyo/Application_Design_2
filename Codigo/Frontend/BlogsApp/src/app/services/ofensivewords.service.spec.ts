import { TestBed } from '@angular/core/testing';

import { OffensivewordsService } from './offensivewords.service';

describe('OfensivewordsService', () => {
  let service: OffensivewordsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OffensivewordsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
