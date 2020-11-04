import { Injectable } from '@angular/core';
import * as signalR from "@aspnet/signalr";

@Injectable({
  providedIn: 'root'
})
export class VehicleNotificationService {
  private hubConnection: signalR.HubConnection

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:5001/hubs/vehicle-notification')
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err))
  }

  public addListener = () => {
    this.hubConnection.on('vehicleNotification', (data) => {
      console.log(data);
    });

    this.hubConnection.send('subscribe', "$$$$$VehicleId$$$");
  }
}
