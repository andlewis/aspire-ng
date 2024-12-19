import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'angularoo';
  forecasts: WeatherForecasts = [];
  constructor(private http:HttpClient){
    console.log('start')
    http.get<WeatherForecasts>('api/weatherforecast').subscribe({
      next: result => {
        this.forecasts = result;
        console.log('result', result);

      },
      error: console.error
    });
    console.log('finished');
  }

  
}

export interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

export type WeatherForecasts = WeatherForecast[];
