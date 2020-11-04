import { Component, OnInit } from '@angular/core';
import { VehicleNotificationService } from '../services/vehicleNotificationService';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  constructor(public vehicleNotificationService: VehicleNotificationService) { }

  ngOnInit() {
    this.vehicleNotificationService.startConnection();
    this.vehicleNotificationService.addListener();
  }
}
