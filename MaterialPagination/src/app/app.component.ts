import { Component, OnInit, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { HttpClient } from '@angular/common/http';
import { MatTableModule } from '@angular/material/table';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, MatPaginatorModule, MatTableModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {
  title = 'MaterialPagination';
  pageSizeOpts = [5, 10, 25, 100];
  displayedCols = ['date', 'temperature', 'summary'];
  totalRows: number = 0;
  totalPages: number = 0;
  forecasts: ForecastDto[] = [];

  http = inject(HttpClient);

  ngOnInit(): void {
    this.getForecasts(1, 10);
  }

  getForecasts(pageIndex: number, pageSize: number) {
    this.http
      .get<PagedData<ForecastDto>>(
        'https://localhost:7202/weatherforecast?pageIndex=' + pageIndex + '&pageSize=' + pageSize
      )
      .subscribe((response) => {
        this.totalRows = response.totalRows;
        this.totalPages = response.totalPages;
        this.forecasts = response.data;
      });
  }

  handlePaging(pageInfo: PageEvent) {
    this.getForecasts(pageInfo.pageIndex, pageInfo.pageSize);
  }
}

interface ForecastDto {
  date: Date;
  temperature: number;
  summary: string;
}

interface PagedData<T> {
  totalRows: number;
  totalPages: number;
  data: T[];
}
