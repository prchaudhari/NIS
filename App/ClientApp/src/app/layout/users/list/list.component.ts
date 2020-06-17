import { Component, OnInit, Injector, ViewChild } from '@angular/core';

import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { HttpClient, HttpResponse, HttpEvent, HttpEventType } from '@angular/common/http';
import { UserService } from '../user.service';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { CellRenderService } from 'src/app/shared/services/cellsrenderer';
import { DialogService } from 'ng2-bootstrap-modal';
import { Constants } from 'src/app/shared/constants/constants';
import { LocalStorageService } from 'src/app/shared/services/local-storage.service';
import { FormGroup, FormBuilder } from '@angular/forms';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { PaginationInstance } from 'src/app/shared/modules/pagination/pagination.module';
import { MessageDialogService } from 'src/app/shared/services/mesage-dialog.service';
import { ConfigConstants } from 'src/app/shared/constants/configConstants';
import { ResourceService } from 'src/app/shared/services/resource.service';
import { User } from '../user';
import { LoginService } from '../../../login/login.service';
@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})

export class ListComponent implements OnInit {
  public isFilter: boolean = false;
  public userList;
  public userLists = [];
  public roleList = [];
  public dataAdapter: any = [];
  public columns = [];
  public actionCellsrenderer: any;
  public createWidget: boolean;
  public message;
  public searchForm: FormGroup;
  public searchedUserList;
  public status;
  public displayRecords;
  public isLoaderActive: boolean = false;
  public params;
  public UserIdentifier;
  public UserName;
  public preferredLanguage;
  public userListResources = {}
  public ResourceLoadingFailedMsg = Constants.ResourceLoadingFailedMsg;
  public Locale;
  public sectionStr;
  public userClaimsRolePrivilegeOperations;
  public roleListsArr: any = [];
  public roleListSource: any[] = []
  public lockSlider: boolean = false;
  public statusSlider: boolean = false;
  public userImage;
  public isRecordFound: boolean;
  public organizationUnitName;
  public emailId;
  public designation;
  public roles;
  public isLocked: boolean
  public isActive: boolean
  public designationList = [];
  public designationLists: any = [];
  public designationListSource: any = [];
  public languageList = [];
  public languageLists: any = [];
  public languageSource: any[] = []
  public ouList: any[] = []
  public ouName;
  public isFilterDone = false;
  displayedColumns: string[] = ['name', 'email', 'mobileno', 'role', 'active', 'lock', 'actions'];
  dataSource = new MatTableDataSource<any>();

  public lockStatusArray: any[] = [
    {
      'Identifier': 1,
      'Name': 'Unlocked'
    },
    {
      'Identifier': 2,
      'Name': 'Locked'
    },
    {
      'Identifier': 3,
      'Name': 'Both'
    }
  ];

  public activeStatusArray: any[] = [
    {
      'Identifier': 1,
      'Name': 'Active'
    },
    {
      'Identifier': 2,
      'Name': 'Inactive'
    },
    {
      'Identifier': 3,
      'Name': 'Both'
    }
  ];

  public array: any;

  public pageSize = 5;
  public currentPage = 0;
  public totalSize = 0;
  public previousPageLabel: string;

  public loggedInUserIdentifier;
  public profileImage;
  public UserFilter: any = {
    FirstName: null,
    LastName: null,
    Code: null,
    EmailAddress: null,
    MobileNumber: null,
    OrganisationUnitIdentifier: null,
    RoleIdentifier: null,
    DesignationIdentifier: null,
    PreferedLanguageIdentifier: null,
    LockStatus: 3,
    ActivationStatus: 3,
  };

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  public handlePage(e: any) {
    this.currentPage = e.pageIndex;
    this.pageSize = e.pageSize;
    this.iterator();
  }

  private iterator() {
    const end = (this.currentPage + 1) * this.pageSize;
    const start = this.currentPage * this.pageSize;
    const part = this.array.slice(start, end);
    this.dataSource = part;
  }

  constructor(private http: HttpClient,
    private service: UserService,
    private router: Router,
    private route: ActivatedRoute,
    private actionCellWindow: CellRenderService,
    private _dialogService: DialogService,
    private localstorageservice: LocalStorageService,
    private fb: FormBuilder,
    private spinner: NgxUiLoaderService,
    private _messageDialogService: MessageDialogService,
    private injector: Injector,
    private loginService: LoginService,
  ) {
    //remove localstorage item.
    router.events.subscribe(e => {
      if (e instanceof NavigationEnd) {
        if (e.url.includes('/user')) {

        } else {
          localStorage.removeItem("userRouteparams")
        }
      }
    });
    //getting localstorage item
    if (localStorage.getItem("userRouteparams")) {
      this.params = JSON.parse(localStorage.getItem('userRouteparams'));
      this.UserIdentifier = this.params.Routeparams.passingparams.UserIdentifier;
      this.UserFilter.FirstName = this.params.Routeparams.filteredparams.FirstName;
      this.UserFilter.LastName = this.params.Routeparams.filteredparams.LastName;
      this.UserFilter.Code = this.params.Routeparams.filteredparams.Code;
      this.UserFilter.EmailAddress = this.params.Routeparams.filteredparams.EmailAddress;
      this.UserFilter.OrganisationUnitIdentifier = this.params.Routeparams.filteredparams.OrganisationUnitIdentifier;
      this.UserFilter.MobileNumber = this.params.Routeparams.filteredparams.MobileNumber;
      this.UserFilter.DesignationIdentifier = this.params.Routeparams.filteredparams.DesignationIdentifier;
      this.UserFilter.RoleIdentifier = this.params.Routeparams.filteredparams.RoleIdentifier;
      this.UserFilter.LockStatus = this.params.Routeparams.filteredparams.LockStatus;
      this.UserFilter.ActivationStatus = this.params.Routeparams.filteredparams.ActivationStatus;
    }
  }

  ngOnInit() {
    // this.getUserdetail();
    this.getRoles();
    var userClaimsDetail = JSON.parse(localStorage.getItem('userClaims'));
    this.userClaimsRolePrivilegeOperations = userClaimsDetail.Privileges;
    this.loggedInUserIdentifier = userClaimsDetail.UserIdentifier;
    this.fetchUserRecord();
  }

  //method binded after html rendering
  ngAfterViewInit(): void {
    
  }

  //Get api for fetching User details--
  async getUserdetail(searchParameter) {
    this.spinner.start();
    if (searchParameter == null) {
      let searchParameter: any = {};
      searchParameter.PagingParameter = {};
      searchParameter.PagingParameter.PageIndex = Constants.DefaultPageIndex;
      searchParameter.PagingParameter.PageSize = Constants.DefaultPageSize;
      searchParameter.SortParameter = {};
      searchParameter.SortParameter.SortColumn = Constants.UserName;
      searchParameter.SortParameter.SortOrder = Constants.Ascending;
      searchParameter.SearchMode = Constants.Contains;
    }
    this.userLists = await this.service.getUser(searchParameter);
    this.userLists.forEach(el => {
      if (el.ProfileImage) {
        if (el.ProfileImage.ThumbnailURL) {
          this.profileImage = el.ProfileImage.ThumbnailURL;
          el.profileImageUrl = el.ProfileImage.ThumbnailURL;
        }
        else {
          el.profileImageUrl = "assets/images/user.jpg";
        }
      }
      else {
        el.profileImageUrl = "assets/images/user.jpg";
      }
     
    })

    // if (this.userLists.length > 0) {
    //     this.isRecordFound = true;
    // }
    if (this.userLists.length == 0 && this.isFilterDone == true) {
      let message = this.userListResources['msgRecordNotFoundError'] == undefined ? this.ResourceLoadingFailedMsg : this.userListResources['msgRecordNotFoundError']
      this._messageDialogService.openDialogBox('Error', message, Constants.msgBoxError).subscribe(data => {
        if (data == true) {
          this.UserFilter.FirstName = null;
          this.UserFilter.LastName = null;
          this.UserFilter.Code = null;
          this.UserFilter.EmailAddress = null;
          this.UserFilter.OrganisationUnitIdentifier = null;
          this.UserFilter.MobileNumber = null;
          this.UserFilter.DesignationIdentifier = null;
          this.UserFilter.RoleIdentifier = null;
          this.UserFilter.PreferedLanguageIdentifier = null;
          this.UserFilter.LockStatus = null;
          this.UserFilter.ActivationStatus = null;
          this.fetchUserRecord();
        }
      });
    }
    this.dataSource = new MatTableDataSource<Element>(this.userLists);
    this.dataSource.paginator = this.paginator;
    this.array = this.userLists;
    this.totalSize = this.array.length;
    this.iterator();
    //to hide tooltip
    const paginatorIntl = this.paginator._intl;
    paginatorIntl.nextPageLabel = '';
    paginatorIntl.previousPageLabel = '';
    paginatorIntl.firstPageLabel = '';
    paginatorIntl.lastPageLabel = '';

    if (this.userLists.length > 0) {
    }
    else {
      this.status = "No Records Found";
    }
  }

  fetchUserRecord() {
    this.params = JSON.parse(localStorage.getItem('userRouteparams'));
    if (localStorage.getItem('userRouteparams')) {
      this.UserIdentifier = this.params.Routeparams.passingparams.UserIdentifier;
      this.UserFilter.FirstName = this.params.Routeparams.filteredparams.FirstName;
      this.UserFilter.LastName = this.params.Routeparams.filteredparams.LastName;
      this.UserFilter.Code = this.params.Routeparams.filteredparams.Code;
      this.UserFilter.EmailAddress = this.params.Routeparams.filteredparams.EmailAddress;
      this.UserFilter.OrganisationUnitIdentifier = this.params.Routeparams.filteredparams.OrganisationUnitIdentifier;
      this.UserFilter.MobileNumber = this.params.Routeparams.filteredparams.MobileNumber;
      this.UserFilter.DesignationIdentifier = this.params.Routeparams.filteredparams.DesignationIdentifier;
      this.UserFilter.RoleIdentifier = this.params.Routeparams.filteredparams.RoleIdentifier;
      this.UserFilter.PreferedLanguageIdentifier = this.params.Routeparams.filteredparams.PreferedLanguageIdentifier;
      this.UserFilter.LockStatus = this.params.Routeparams.filteredparams.LockStatus;
      this.UserFilter.ActivationStatus = this.params.Routeparams.filteredparams.ActivationStatus;
    }
    let searchParameter: any = {};
    searchParameter.PagingParameter = {};
    searchParameter.PagingParameter.PageIndex = Constants.DefaultPageIndex;
    searchParameter.PagingParameter.PageSize = Constants.DefaultPageSize;
    searchParameter.SortParameter = {};
    searchParameter.SortParameter.SortColumn = Constants.UserName;
    searchParameter.SortParameter.SortOrder = Constants.Ascending;
    searchParameter.SearchMode = Constants.Contains;
    searchParameter.GetResources = true;
    if (this.UserFilter.FirstName != null) {
      searchParameter.FirstName = this.UserFilter.FirstName;
    }
    if (this.UserFilter.LastName != null) {
      searchParameter.LastName = this.UserFilter.LastName;
    }
    if (this.UserFilter.Code != null) {
      searchParameter.Code = this.UserFilter.Code;
    }
    if (this.UserFilter.EmailAddress != null) {
      searchParameter.EmailAddress = this.UserFilter.EmailAddress;
    }
    if (this.UserFilter.MobileNumber != null) {
      searchParameter.MobileNumber = this.UserFilter.MobileNumber;
    }
    if (this.UserFilter.OrganisationUnitIdentifier != null) {
      searchParameter.OrganisationUnitIdentifier = this.UserFilter.OrganisationUnitIdentifier;
    }
    if (this.UserFilter.DesignationIdentifier != null) {
      searchParameter.DesignationIdentifier = this.UserFilter.DesignationIdentifier;
    }
    if (this.UserFilter.PreferedLanguageIdentifier != null) {
      searchParameter.PreferedLanguageIdentifier = this.UserFilter.PreferedLanguageIdentifier;
    }
    if (this.UserFilter.RoleIdentifier != null) {
      searchParameter.RoleIdentifier = this.UserFilter.RoleIdentifier;
    }
    searchParameter.LockStatus = this.UserFilter.LockStatus;
    searchParameter.ActivationStatus = this.UserFilter.ActivationStatus;
    this.getUserdetail(searchParameter);
  }
  async getRoles() {
    let searchParameter: any = {};
    searchParameter.PagingParameter = {};
    searchParameter.PagingParameter.PageIndex = Constants.DefaultPageIndex;
    searchParameter.PagingParameter.PageSize = Constants.DefaultPageSize;
    searchParameter.SortParameter = {};
    searchParameter.SortParameter.SortColumn = Constants.Name;
    searchParameter.SortParameter.SortOrder = Constants.Ascending;
    searchParameter.SearchMode = Constants.Contains;
    //searchParameter.GetPrivileges = true;
    this.roleList = await this.loginService.getRoles(searchParameter);
    
  }
  //Function to navigate to view page of perticular user detail--
  viewUser(user) {
    let queryParams = {
      Routeparams: {
        passingparams: {
          "UserIdentifier": user.Identifier,
        },
        filteredparams: {
          //passing data using json stringify.
          "FirstName": this.UserFilter.FirstName != null ? this.UserFilter.FirstName : null,
          "LastName": this.UserFilter.LastName != null ? this.UserFilter.LastName : null,
          "Code": this.UserFilter.Code != null ? this.UserFilter.Code : null,
          "EmailAddress": this.UserFilter.EmailAddress != null ? this.UserFilter.EmailAddress : null,
          "MobileNumber": this.UserFilter.MobileNumber != null ? this.UserFilter.MobileNumber : null,
          "OrganisationUnitIdentifier": this.UserFilter.OrganisationUnitIdentifier != null ? this.UserFilter.OrganisationUnitIdentifier : null,
          "DesignationIdentifier": this.UserFilter.DesignationIdentifier != null ? this.UserFilter.DesignationIdentifier : null,
          "PreferedLanguageIdentifier": this.UserFilter.PreferedLanguageIdentifier != null ? this.UserFilter.PreferedLanguageIdentifier : null,
          "RoleIdentifier": this.UserFilter.RoleIdentifier != null ? this.UserFilter.RoleIdentifier : null,
          "LockStatus": this.UserFilter.LockStatus,
          "ActivationStatus": this.UserFilter.ActivationStatus,
        }
      }
    }
    localStorage.setItem("userRouteparams", JSON.stringify(queryParams))
    this.router.navigate(['user', 'userView']);
  }

  //Function to edit perticular user--
  editUser(user) {
    
    let queryParams = {
      Routeparams: {
        passingparams: {
          "UserIdentifier": user.Identifier,
        },
        filteredparams: {
          //passing data using json stringify.
          "FirstName": this.UserFilter.FirstName != null ? this.UserFilter.FirstName : null,
          "LastName": this.UserFilter.LastName != null ? this.UserFilter.LastName : null,
          "EmailAddress": this.UserFilter.EmailAddress != null ? this.UserFilter.EmailAddress : null,
          "MobileNumber": this.UserFilter.MobileNumber != null ? this.UserFilter.MobileNumber : null,
          "RoleIdentifier": this.UserFilter.RoleIdentifier != null ? this.UserFilter.RoleIdentifier : null,
        }
      }
    }
    localStorage.setItem("userRouteparams", JSON.stringify(queryParams))
    this.router.navigate(['user', 'userEdit']);
  }

  //function written to delete user--
  deleteUser(user: User) {
    let message = this.userListResources['msgConfirmation'] == undefined ? this.ResourceLoadingFailedMsg : this.userListResources['msgConfirmation']
    this._messageDialogService.openConfirmationDialogBox('Confirm', message, Constants.msgBoxWarning).subscribe(async (isConfirmed) => {
      if (isConfirmed) {
        //let isDependencyPresent = await this.service.checkDependency(user.Identifier);
        //if (isDependencyPresent) {
        //    let msg = this.userListResources['msgDependencyPresent'] == undefined ? this.ResourceLoadingFailedMsg : this.userListResources['msgDependencyPresent'];
        //    this._messageDialogService.openDialogBox('Error', msg, Constants.msgBoxError);
        //}
        //else {
        let isDeleted = await this.service.deleteUser(user.Identifier);
        if (isDeleted) {
          let messageString = Constants.recordDeletedMessage;
          this._messageDialogService.openDialogBox('Success', messageString, Constants.msgBoxSuccess);
          this.fetchUserRecord();
        }
        //}
      }
    });
  }

  //function written to unlock user--
  unLockUser(user: User) {
    let message = this.userListResources['msgUnlockConfirmation'] == undefined ? this.ResourceLoadingFailedMsg : this.userListResources['msgUnlockConfirmation']
    this._messageDialogService.openConfirmationDialogBox('Confirm', message, Constants.msgBoxWarning).subscribe(async (isConfirmed) => {
      if (isConfirmed) {
        this.isLoaderActive = true;
        let isDeleted = await this.service.unlockUser(user.Identifier);
        if (isDeleted) {
          let messageString = Constants.recordUnlockedMessage;
          this._messageDialogService.openDialogBox('Success', messageString, Constants.msgBoxSuccess);
          this.fetchUserRecord();
        }
      }
    });
  }

  //this method helps to navigate to add
  navigateToAddUser() {
    this.router.navigate(['user', 'userAdd']);
  }

  //User filter function--
  filterSetUp(searchType) {
    this.isFilterDone = true;
    if (searchType == 'Reset') {
      localStorage.removeItem("userRouteparams");
      this.UserFilter = {
        FirstName: null,
        LastName: null,
        Code: null,
        EmailAddress: null,
        MobileNumber: null,
        OrganisationUnitIdentifier: null,
        RoleIdentifier: null,
        LockStatus: 0,
        ActivationStatus: 0,
      };
     
      this.isFilter = !this.isFilter;
      this.fetchUserRecord();
      this.userLists = [];
      this.searchedUserList = [];
    }
    else {
      let searchParameter: any = {};
      searchParameter.PagingParameter = {};
      searchParameter.PagingParameter.PageIndex = Constants.DefaultPageIndex;
      searchParameter.PagingParameter.PageSize = Constants.DefaultPageSize;
      searchParameter.SortParameter = {};
      searchParameter.SortParameter.SortColumn = Constants.UserName;
      searchParameter.SortParameter.SortOrder = Constants.Ascending;
      searchParameter.SearchMode = Constants.Contains;
      if (this.UserFilter.FirstName != null) {
        searchParameter.FirstName = this.UserFilter.FirstName;
      }
      if (this.UserFilter.EmailAddress != null) {
        searchParameter.EmailAddress = this.UserFilter.EmailAddress;
      }
      if (this.UserFilter.RoleIdentifier != null) {
        searchParameter.RoleIdentifier = this.UserFilter.RoleIdentifier;
      }
      searchParameter.LockStatus = this.UserFilter.LockStatus;
      searchParameter.ActivationStatus = this.UserFilter.ActivationStatus;
      this.getUserdetail(searchParameter);
      this.isFilter = !this.isFilter;
    }
  }

  //Function to close the filter popup--
  closeFilter() {
    this.isFilter = !this.isFilter;
  }
}
