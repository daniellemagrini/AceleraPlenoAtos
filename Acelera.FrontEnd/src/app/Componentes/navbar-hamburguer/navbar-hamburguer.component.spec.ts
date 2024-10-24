import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NavbarHamburguerComponent } from './navbar-hamburguer.component';

describe('NavbarHamburguerComponent', () => {
  let component: NavbarHamburguerComponent;
  let fixture: ComponentFixture<NavbarHamburguerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NavbarHamburguerComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(NavbarHamburguerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
