import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContentToRevisionComponent } from './content-to-revision.component';

describe('ContentToRevisionComponent', () => {
  let component: ContentToRevisionComponent;
  let fixture: ComponentFixture<ContentToRevisionComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ContentToRevisionComponent]
    });
    fixture = TestBed.createComponent(ContentToRevisionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
