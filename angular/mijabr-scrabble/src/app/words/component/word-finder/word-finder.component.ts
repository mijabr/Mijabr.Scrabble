import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { WordService } from '../../service/word.service';

@Component({
  selector: 'app-word-finder',
  templateUrl: './word-finder.component.html',
  styleUrls: ['./word-finder.component.less']
})
export class WordFinderComponent implements OnInit {

  public wordForm = this.fb.group({
    pattern: ['', Validators.required],
    letters: ['', ]
  });

  result: any;

  constructor(public fb: FormBuilder, private wordService: WordService) {
  }

  ngOnInit() {
  }

  onFindClick(event) {
    this.result = undefined;

    if (this.wordForm.value.letters.length === 0) {
      this.findWords();
    } else {
      this.findWordsUsingLetters();
    }
  }

  findWords() {
    this.wordService.findWords(this.wordForm.value.pattern)
      .subscribe((response: any) => {
        this.result = response;
        this.result.searchPattern = this.result.searchPattern.replace(new RegExp('\\$', 'g'), '?');
      });
  }

  findWordsUsingLetters() {
    this.wordService.findWordsUsingLetters(this.wordForm.value.pattern, this.wordForm.value.letters)
      .subscribe((response: any) => {
        this.result = response;
        this.result.searchPattern = this.result.searchPattern.replace(new RegExp('\\$', 'g'), '?');
      });
  }
}
