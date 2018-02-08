import { TestBed, inject } from '@angular/core/testing';

import { MetamaskAccountService } from './metamask-account.service';

describe('MetamaskAccountService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [MetamaskAccountService]
    });
  });

  it('should be created', inject([MetamaskAccountService], (service: MetamaskAccountService) => {
    expect(service).toBeTruthy();
  }));
});
