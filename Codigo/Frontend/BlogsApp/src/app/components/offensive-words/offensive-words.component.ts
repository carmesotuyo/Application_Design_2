import { Component, OnInit } from '@angular/core';
import { OffensivewordsService } from 'src/app/services/offensivewords.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ValidateString } from 'src/app/validators/string.validator';

@Component({
  selector: 'app-offensive-words',
  templateUrl: './offensive-words.component.html',
  styleUrls: ['./offensive-words.component.scss']
})
export class OffensiveWordsComponent implements OnInit {
  words: string[] = [];
  form: FormGroup;

  constructor(private wordsService: OffensivewordsService) { 
    this.form = new FormGroup({
      word: new FormControl('', [Validators.required, ValidateString]),
    });
  }

  ngOnInit() {
    this.getWords();
  }

  getWords() {
    this.wordsService.getOfensiveWords().subscribe((words: string[]) => {
      this.words = words;
    });
  }

  addWord() {
    if (this.form.valid) {
      const newWord = this.form.value.word;
      this.wordsService.postOffensiveWord(newWord).subscribe(() => {
        this.getWords();
        this.form.reset();
      });
    }
  }

  deleteWord(word: string) {
    this.wordsService.deleteOffensiveWord(word).subscribe(() => {
      this.getWords();
    });
  }
}
