import { TestBed, inject } from '@angular/core/testing';

import { MetamaskService } from './metamask.service';

describe('MetamaskService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [MetamaskService]
    });
  });

  it('should be created', inject([MetamaskService], (service: MetamaskService) => {
    expect(service).toBeTruthy();
  }));
});
