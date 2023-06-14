import { Component, OnInit } from '@angular/core';
import { ArticleService } from 'src/app/services/article.service';
import { FormControl } from '@angular/forms';

interface MonthlyStatistics {
  month: string;
  count: number;
}

@Component({
  selector: 'app-stats',
  templateUrl: './stats.component.html',
  styleUrls: ['./stats.component.scss']
})
export class StatsComponent implements OnInit {
  year = new FormControl((new Date()).getFullYear());
  statistics: MonthlyStatistics[] = [];

  displayedColumns: string[] = ['month', 'count'];

  constructor(private articleService: ArticleService) { }

  ngOnInit(): void { }

  getStatistics(): void {
    this.articleService.getStats(this.year.value!).subscribe((statistics: any) => {
      this.statistics = Object.keys(statistics).map((month) => ({
        month: month,
        count: statistics[month],
      }));
    });
  }
}
