import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { bloggerGuard } from './blogger.guard';

describe('bloggerGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => bloggerGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
