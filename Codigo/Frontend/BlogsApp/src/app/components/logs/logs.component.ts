import { Component, OnInit } from '@angular/core';
import { Logs } from 'src/app/models/logs.model';
import { LogsService } from 'src/app/services/logs.service';
import { FormControl } from '@angular/forms';
import { formatDate } from '@angular/common';

@Component({
  selector: 'app-logs',
  templateUrl: './logs.component.html',
  styleUrls: ['./logs.component.scss']
})
export class LogsComponent implements OnInit {
  displayedColumns: string[] = ['id', 'userId', 'actionType', 'searchQuery', 'timestamp'];

  fromDate = new FormControl(new Date());
  toDate = new FormControl(new Date());

  logs: Logs[] = [];

  constructor(private logsService: LogsService) { }

  ngOnInit(): void { }

  getLogs(): void {
    const from = formatDate(this.fromDate.value!, 'yyyy-MM-dd', 'en');
    const to = formatDate(this.toDate.value!, 'yyyy-MM-dd', 'en');

    this.logsService.getLogs(from, to).subscribe((logs: Logs[]) => {
      this.logs = logs;
    });
  }
}
