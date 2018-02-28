import { Component, OnInit, Output, Inject, EventEmitter } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material"

@Component({
  selector: 'app-set-hash-popup',
  templateUrl: './set-hash-popup.component.html',
  styleUrls: ['./set-hash-popup.component.css']
})
export class SetHashPopupComponent implements OnInit {
  @Output() onSendClick = new EventEmitter<string>();
  constructor(private dialogRef: MatDialogRef<SetHashPopupComponent>,
    @Inject(MAT_DIALOG_DATA) public data) {
  }

  ngOnInit() {
  }
}
