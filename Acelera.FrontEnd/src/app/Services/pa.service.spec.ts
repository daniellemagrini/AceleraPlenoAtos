import { TestBed } from '@angular/core/testing';

import { PaService } from './pa.service';

describe('PaService', () => {
  let service: PaService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PaService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
