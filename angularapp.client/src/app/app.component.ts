import { Component } from '@angular/core';
import { PositionsComponent } from './components/positions/positions.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [PositionsComponent],   
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Position Managemnet';
}
