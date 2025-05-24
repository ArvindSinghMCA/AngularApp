import { Component, OnInit } from '@angular/core';
import { PositionsService, Position } from '../../services/positions.service';
import { CommonModule } from '@angular/common';  // For *ngIf, *ngFor

@Component({
  selector: 'app-positions',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './positions.component.html',
  styleUrls: ['./positions.component.css']
})
export class PositionsComponent implements OnInit {
  positions: Position[] = [];
  loading = false;
  error: string | null = null;

  constructor(private positionsService: PositionsService) { }

  ngOnInit(): void {
    this.loading = true;
    this.positionsService.getPositions().subscribe({
      next: (data) => {
        this.positions = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load positions';
        this.loading = false;
      }
    });
  }
}

