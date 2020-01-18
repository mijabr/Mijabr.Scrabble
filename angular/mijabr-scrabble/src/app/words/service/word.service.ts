import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../../service/api.service';

@Injectable()
export class WordService {

  isWordToggle = false;
  findWordsToggle = false;

  constructor(
    private apiService: ApiService
  ) { }

  isWord(word: string): Observable<any> {
    return this.apiService.post('scrabble/api/word/isword', {'word': word});
  }

  findWords(pattern: string): Observable<any> {
    pattern = pattern.replace(new RegExp('\\?', 'g'), '$');
    return this.apiService.post('scrabble/api/word/findwords', {'pattern': pattern});
  }

  findWordsUsingLetters(pattern: string, letters: string): Observable<any> {
    pattern = pattern.replace(new RegExp('\\?', 'g'), '$');
    return this.apiService.post('scrabble/api/word/findwords', {'pattern': pattern, 'letters': letters});
  }
}
