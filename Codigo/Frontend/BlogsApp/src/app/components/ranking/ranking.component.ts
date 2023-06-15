import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { UserRanking } from 'src/app/models/user.model';
import { FormControl } from '@angular/forms';
import { formatDate } from '@angular/common';

@Component({
  selector: 'app-ranking',
  templateUrl: './ranking.component.html',
  styleUrls: ['./ranking.component.scss']
})
export class RankingComponent {
  displayedColumns: string[] = ['id', 'username'];

  fromDate = new FormControl(new Date());
  toDate = new FormControl(new Date());
  ranking: number = 0;

  users: UserRanking[] = [];

  constructor(private userService: UserService) { }

  ngOnInit(): void { }

  getNormalRanking(): void {
    const from = formatDate(this.fromDate.value!, 'yyyy-MM-dd', 'en');
    const to = formatDate(this.toDate.value!, 'yyyy-MM-dd', 'en');

    this.userService.getRanking(from, to, this.ranking).subscribe((users: UserRanking[]) => {
      this.users = users;
    });
  }

  getOffensiveWordsRanking(): void {
    const from = formatDate(this.fromDate.value!, 'yyyy-MM-dd', 'en');
    const to = formatDate(this.toDate.value!, 'yyyy-MM-dd', 'en');

    this.userService.getRanking(from, to, this.ranking, true).subscribe((users: UserRanking[]) => {
      this.users = users;
    });
  }
}
