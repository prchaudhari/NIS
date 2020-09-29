import { Component, OnInit, SecurityContext } from '@angular/core';
import { CompactType, DisplayGrid, GridsterConfig, GridsterItem, GridType } from 'angular-gridster2';
import { MessageDialogService } from 'src/app/shared/services/mesage-dialog.service';
import { DialogComponent, DialogService } from '@tomblue/ng2-bootstrap-modal';
import { ConfigConstants } from '../../shared/constants/configConstants';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import * as $ from 'jquery';
import { DomSanitizer } from '@angular/platform-browser';
import { SavingTrendsPreviewComponent, SpendindTrendsPreviewComponent, AnalyticsWidgetPreviewComponent } from '../widgetComponent/widgetComponent';

//If you change dashboard-container class in below HTML template, 
//then you can also need to change in applyBackgroundImage to replace new class name over there.
@Component({
  selector: 'app-page-design-preview',
  template: `<div class="modal-dialog modal-dialog-centered modal-xl" role="document">
    <div class="modal-content overflow-hidden">
      <div class="modal-body p-1 text-center">
        <button type="button" class="close p-1 mt-n2" (click)="cancel()">
          <span aria-hidden="true">&times;</span>
        </button>
        <div class="dashboard-container p-1">
            <div class="dashboard-inner-container ml-1 mr-1 overflow-auto stylescrollbar w-100">
                <div class="widget-container">
                  <gridster [options]="options">
                  <gridster-item [item]="item" *ngFor="let item of ItemArray">
                      <vidyo-widget [vdoItem]="item" *ngIf="item.value=='Video'"></vidyo-widget>
                      <image-widget [imgItem]="item" *ngIf="item.value=='Image'"></image-widget>
                      <ndc-dynamic [ndcDynamicComponent]="item.component"></ndc-dynamic>
                  </gridster-item>
                  </gridster>
                </div>
            </div>
        </div>
      </div>
      <div class="modal-footer"></div>
    </div>
  </div>`
})

export class PageDesignPreviewComponent extends DialogComponent<PageDesignPreviewModel, boolean> implements PageDesignPreviewModel, DialogOptions, OnInit {

  widgetItemArray:any[];
  ItemArray:any[];
  BackgroundImageAssetId: number;
  BackgroundImageURL: string;
  backdropColor: string = "red";
  public options: GridsterConfig;
  dashboard: Array<GridsterItem>;
  item: any[];
  public baseURL: string = ConfigConstants.BaseURL;

  constructor(dialogService: DialogService,
    private _messageDialogService: MessageDialogService,
    private _http: HttpClient,
    private sanitizer: DomSanitizer) { 
      super(dialogService);
    }

  ngOnInit() {

    this.applyBackgroundImage(this.BackgroundImageAssetId, this.BackgroundImageURL);

    this.ItemArray = [];
    for(let i=0; i< this.widgetItemArray.length; i++) {
      let widget = Object.assign({}, this.widgetItemArray[i]);
      if(widget.widgetId == 9 || widget.widgetId == 6 || widget.widgetId == 14) {
        widget.component = this.bindComponent(widget.widgetId)
      }
      this.ItemArray.push(widget);
    }

    //gridster
    this.options = {
      gridType: GridType.VerticalFixed,
      compactType: CompactType.None,
      margin: 10,
      outerMargin: true,
      outerMarginTop: null,
      outerMarginRight: null,
      outerMarginBottom: null,
      outerMarginLeft: null,
      useTransformPositioning: true,
      mobileBreakpoint: 640,
      minCols: 12,
      maxCols: 12,
      minRows: 20,
      maxRows: 100,
      maxItemCols: 100,
      minItemCols: 1,
      maxItemRows: 100,
      minItemRows: 1,
      maxItemArea: 2500,
      minItemArea: 1,
      defaultItemCols: 1,
      defaultItemRows: 1,
      fixedColWidth: 175,
      fixedRowHeight: 105,
      keepFixedHeightInMobile: false,
      keepFixedWidthInMobile: false,
      scrollSensitivity: 10,
      scrollSpeed: 20,
      enableEmptyCellClick: false,
      enableEmptyCellContextMenu: false,
      enableEmptyCellDrop: false,
      enableEmptyCellDrag: false,
      emptyCellDragMaxCols: 10,
      emptyCellDragMaxRows: 10,
      ignoreMarginInRow: false,
      draggable: {
        enabled: false,
      },
      resizable: {
        enabled: false,
      },
      swap: false,
      pushItems: false,
      disablePushOnDrag: false,
      disablePushOnResize: false,
      pushDirections: { north: false, east: false, south: false, west: false },
      pushResizeItems: false,
      displayGrid: DisplayGrid.None,
      disableWindowResize: false,
      disableWarnings: false,
      scrollToNewItems: true
    };

  }

  bindComponent(widgetId): any {
    if (widgetId == 9) {
      return SavingTrendsPreviewComponent;
    }else if(widgetId == 6) {
      return AnalyticsWidgetPreviewComponent;
    }else if(widgetId == 14) {
      return SpendindTrendsPreviewComponent;
    }
}

  cancel() {
    this.close();
  }

  applyBackgroundImage(AssetId, ImageURL) {
    if(AssetId != null && AssetId != 0) {
      this._http.get(this.baseURL + 'assetlibrary/asset/download?assetIdentifier=' + AssetId, { responseType: "arraybuffer", observe: 'response' }).pipe(map(response => response))
          .subscribe(
            data => {
              let contentType = data.headers.get('Content-Type');
              const blob = new Blob([data.body], { type: contentType });
              let objectURL = URL.createObjectURL(blob);
              let imgUrl = this.sanitizer.sanitize(SecurityContext.RESOURCE_URL, this.sanitizer.bypassSecurityTrustResourceUrl(objectURL));
              $('.dashboard-container').css('background', 'url('+imgUrl+')');
            },
            error => {
              //$('.overlay').show();
          });
    }else if(ImageURL != '') {
      $('.dashboard-container').css('background', 'url('+ImageURL+')');
    }    
  }

}

export interface PageDesignPreviewModel {
  widgetItemArray:any[];
  BackgroundImageAssetId: number;
  BackgroundImageURL: string;
}

interface DialogOptions {
  backdropColor?: string;
}
