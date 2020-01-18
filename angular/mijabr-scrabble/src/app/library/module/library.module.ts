import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import {
  CardComponent, CardHeaderDirective, CardBodyTitleDirective, CardBodyDirective, CardFooterDirective, CardLeftDirective
} from '../component/card/card.component';
import { ExpanderButtonComponent } from '../component/expander-button/expander-button/expander-button.component';
import { IconHomeComponent } from '../component/icon/icon-home';
import { IconWeightComponent } from '../component/icon/icon-weight';
import { IconThumbUpComponent } from '../component/icon/icon-thumb-up';
import { IconArrowDownBoldComponent } from '../component/icon/icon-arrow-down-bold';
import { IconStarComponent } from '../component/icon/icon-star';
import { ButtonComponent } from '../component/button/button.component';
import { IconAccountComponent } from '../component/icon/icon-account';
import { ModalComponent } from '../component/modal/modal.component';
import { LinkComponent } from '../component/link/link.component';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [
    CardComponent, CardHeaderDirective, CardBodyTitleDirective, CardBodyDirective, CardFooterDirective, CardLeftDirective,
    ExpanderButtonComponent, ButtonComponent, LinkComponent,
    IconHomeComponent, IconWeightComponent, IconThumbUpComponent, IconArrowDownBoldComponent, IconStarComponent, IconAccountComponent,
    ModalComponent
  ],
  exports: [
    CardComponent, CardHeaderDirective, CardBodyTitleDirective, CardBodyDirective, CardFooterDirective, CardLeftDirective,
    ExpanderButtonComponent, ButtonComponent, LinkComponent,
    IconHomeComponent, IconWeightComponent, IconThumbUpComponent, IconArrowDownBoldComponent, IconStarComponent, IconAccountComponent,
    ModalComponent
]
})
export class LibraryModule { }
