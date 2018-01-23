import { TestBed, inject } from '@angular/core/testing';

import { AdvisorService } from './advisor.service';

describe('AdvisorService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AdvisorService]
    });
  });

  it('should be created', inject([AdvisorService], (service: AdvisorService) => {
    expect(service).toBeTruthy();
  }));
});
