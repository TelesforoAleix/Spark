import { Component, OnInit } from '@angular/core';
import { Attendee } from '../model/attendee';
import { AttendeeService } from '../services/attendee.service';
import { Router } from '@angular/router';
import { NgIf, NgForOf } from '@angular/common';

@Component({
  selector: 'app-attendee-list',
  standalone: true,
  imports: [NgIf, NgForOf],
  templateUrl: './attendee-list.component.html',
  styleUrl: './attendee-list.component.css'
})
export class AttendeeListComponent implements OnInit {
  attendees: Attendee[] = [];
  filteredAttendees: Attendee[] = [];
  searchTerm: string = '';
  viewMode: 'table' | 'card' = 'table';
  sortField: string = 'name';
  sortDirection: 'asc' | 'desc' = 'asc';
  
  // For the delete confirmation modal
  showDeleteModal: boolean = false;
  selectedAttendee: Attendee | null = null;
  
  // For pagination
  currentPage: number = 1;
  itemsPerPage: number = 10;
  
  get totalPages(): number {
    return Math.ceil(this.filteredAttendees.length / this.itemsPerPage);
  }
  
  get paginatedAttendees(): Attendee[] {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    return this.filteredAttendees.slice(startIndex, startIndex + this.itemsPerPage);
  }

  constructor(
    private attendeeService: AttendeeService, 
    private router: Router
  ) {}

  ngOnInit(): void {
    if (this.attendeeService.authHeader == null) { 
      this.router.navigate(["login"]); 
      return; 
  }
    this.loadAttendees();
  }
  
  loadAttendees(): void {
    this.attendeeService.getAttendees().subscribe(
      (data) => {
        this.attendees = data;
        this.applyFilters();
      },
      (error) => {
        console.error('Error fetching attendees:', error);
      }
    );
  }
  
  filterAttendees(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.searchTerm = input.value.toLowerCase();
    this.applyFilters();
  }
  
  applyFilters(): void {
    this.filteredAttendees = this.attendees.filter(attendee => 
      attendee.firstName.toLowerCase().includes(this.searchTerm) || 
      attendee.lastName.toLowerCase().includes(this.searchTerm) ||
      attendee.email.toLowerCase().includes(this.searchTerm) ||
      attendee.header.toLowerCase().includes(this.searchTerm)
    );
    
    this.sortAttendees(this.sortField, false);
  }
  
  sortAttendees(field: string, toggleDirection: boolean = true): void {
    if (toggleDirection && this.sortField === field) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    }
    
    this.sortField = field;
    
    this.filteredAttendees.sort((a, b) => {
      let valueA: string;
      let valueB: string;
      
      if (field === 'name') {
        valueA = `${a.firstName} ${a.lastName}`.toLowerCase();
        valueB = `${b.firstName} ${b.lastName}`.toLowerCase();
      } else if (field === 'header') {
        valueA = a.header.toLowerCase();
        valueB = b.header.toLowerCase();
      } else {
        valueA = (a as any)[field]?.toString().toLowerCase() || '';
        valueB = (b as any)[field]?.toString().toLowerCase() || '';
      }
      
      if (this.sortDirection === 'asc') {
        return valueA.localeCompare(valueB);
      } else {
        return valueB.localeCompare(valueA);
      }
    });
  }
  
  confirmDelete(attendee: Attendee): void {
    this.selectedAttendee = attendee;
    this.showDeleteModal = true;
  }

  deleteAttendee(attendeeId: string): void {
    if (attendeeId) {
      this.attendeeService.deleteAttendee(attendeeId).subscribe(
        () => {
          this.attendees = this.attendees.filter(a => a.attendeeId !== attendeeId);
          this.applyFilters();
          this.showDeleteModal = false;
        },
        (error) => {
          console.error('Error deleting attendee:', error);
        }
      );
    }
  }

  editAttendee(attendeeId: string): void {
    if (attendeeId) {
      this.router.navigate(['/edit-attendee', attendeeId]);
    }
  }
  
  addNewAttendee(): void {
    this.router.navigate(['/add-attendee']);
  }
  


}
