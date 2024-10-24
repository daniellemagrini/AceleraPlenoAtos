import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaComponent } from './pa.component';

describe('PaComponent', () => {
  let component: PaComponent;
  let fixture: ComponentFixture<PaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PaComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
