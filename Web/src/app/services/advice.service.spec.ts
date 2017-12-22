import { TestBed, inject } from '@angular/core/testing';

import { AdviceService } from './advice.service';

describe('AdviceService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AdviceService]
    });
  });

  it('should be created', inject([AdviceService], (service: AdviceService) => {
    expect(service).toBeTruthy();
  }));
});
