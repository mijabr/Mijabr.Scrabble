import {
  Component,
  Input,
  Directive,
  ContentChild
} from '@angular/core';

@Directive({ selector: 'app-card-header' })
export class CardHeaderDirective {
  constructor() { }
}

@Directive({ selector: 'app-card-body-title' })
export class CardBodyTitleDirective {
  constructor() { }
}

@Directive({ selector: 'app-card-body' })
export class CardBodyDirective {
  constructor() { }
}

@Directive({ selector: 'app-card-footer' })
export class CardFooterDirective {
  constructor() { }
}

@Directive({ selector: 'app-card-left' })
export class CardLeftDirective {
  constructor() { }
}

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.less']
})

export class CardComponent {
  @ContentChild(CardHeaderDirective, null) cardHeader: CardHeaderDirective;
  @ContentChild(CardBodyTitleDirective, null) cardBodyTitle: CardBodyTitleDirective;
  @ContentChild(CardFooterDirective, null) cardFooter: CardFooterDirective;
  @ContentChild(CardLeftDirective, null) cardLeft: CardLeftDirective;
  @Input() contentHeight?: string;
  @Input() transition?: string;
  @Input() maxHeight?: string;
}
