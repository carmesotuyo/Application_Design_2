import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditAllUsersComponent } from './edit-all-users.component';

describe('EditAllUsersComponent', () => {
  let component: EditAllUsersComponent;
  let fixture: ComponentFixture<EditAllUsersComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EditAllUsersComponent]
    });
    fixture = TestBed.createComponent(EditAllUsersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
