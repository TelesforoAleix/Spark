import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddAttendeeComponent } from './add-attendee.component';

describe('AddAttendeeComponent', () => {
  let component: AddAttendeeComponent;
  let fixture: ComponentFixture<AddAttendeeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddAttendeeComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddAttendeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
