import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RedefinicaoSenhaComponent } from './redefinicaoSenha.component';

describe('RedefinicaoSenhaComponent', () => {
  let component: RedefinicaoSenhaComponent;
  let fixture: ComponentFixture<RedefinicaoSenhaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RedefinicaoSenhaComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RedefinicaoSenhaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
