import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VerifyEmailForm } from './verify-email-form';

describe('VerifyEmailForm', () => {
  let component: VerifyEmailForm;
  let fixture: ComponentFixture<VerifyEmailForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VerifyEmailForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VerifyEmailForm);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
