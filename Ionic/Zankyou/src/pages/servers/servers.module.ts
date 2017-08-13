import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ServersPage } from './servers';

@NgModule({
  declarations: [
    ServersPage,
  ],
  imports: [
    IonicPageModule.forChild(ServersPage),
  ],
})
export class ServersPageModule {
  constructor() {
    
  }
}
