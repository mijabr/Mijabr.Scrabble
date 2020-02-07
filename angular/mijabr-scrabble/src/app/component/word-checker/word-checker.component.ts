import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { WordService } from '../../service/word.service';

@Component({
  selector: 'app-word-checker',
  templateUrl: './word-checker.component.html',
  styleUrls: ['./word-checker.component.less']
})
export class WordCheckerComponent implements OnInit {

  public wordForm = this.fb.group({
    word: ['', Validators.required]
  });

  result: any;

  constructor(
    public fb: FormBuilder,
    private wordService: WordService
  ) { }

  ngOnInit() { }

  checkWord(event) {
    this.result = undefined;
    this.wordService.isWord(this.wordForm.value.word)
      .subscribe((response: any) => {
        this.result = response;
      });
  }
}
