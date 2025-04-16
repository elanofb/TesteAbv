import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="app-header">
      <h1>DeveloperStore</h1>
    </div>
  `,
  styles: [`
    .app-header {
      background-color: #2c3e50;
      color: white;
      padding: 15px 0;
      text-align: center;
      margin-bottom: 20px;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
      width: 100%;
    }
    .app-header h1 {
      margin: 0;
      font-size: 24px;
      font-weight: 500;
    }
  `]
})
export class HeaderComponent {}