import { Component, OnInit, Injector } from '@angular/core';
import { Constants, DynamicGlobalVariable } from 'src/app/shared/constants/constants';
import { LoginService } from './../login/login.service';
import { Router } from '@angular/router';
import { LocalStorageService } from 'src/app/shared/services/local-storage.service';
import { MessageDialogService } from 'src/app/shared/services/mesage-dialog.service';
import { MultiTenantUserAccessMapService } from '../layout/multi-tenant-user-access-map/multi-tenant-user-access-map.service';

@Component({
  selector: 'app-select-tenant',
  templateUrl: './select-tenant.component.html',
  styleUrls: ['./select-tenant.component.scss']
})
export class SelectTenantComponent implements OnInit {

  public tenants: any[] = [];
  public roleList: any = [];
  public commonRolePrivileges = [];
  public roleDetail;
  public userData;
  public statePrivilegeMap: any = [{
    "State": "dashboard",
    "Entity": "Dashboard",
  },
  {
    "State": "user",
    "Entity": "User",
  },
  {
    "State": "dashboard",
    "Entity": "Dashboard",
  },
  {
    "State": "roles",
    "Entity": "Role",
  },
  {
    "State": "assetlibrary",
    "Entity": "Asset Library",
  },
  {
    "State": "widgets",
    "Entity": "Widget",
  },
  {
    "State": "pages",
    "Entity": "Page",
  },
  {
    "State": "statementdefination",
    "Entity": "Statement Definition",
  },
  {
    "State": "schedulemanagement",
    "Entity": "Schedule Management",
  },
  {
    "State": "logs",
    "Entity": "Log",
  },
  {
    "State": "analytics",
    "Entity": "Analytics",
  },
  {
    "State": "statemenetsearch",
    "Entity": "Statement Search",
  },
  ]

  constructor(private injector: Injector,
    private loginService: LoginService,
    private route: Router,
    private _messageDialogService: MessageDialogService,
    private localstorageservice: LocalStorageService) { }

  ngOnInit() {
    this.userData = JSON.parse(localStorage.getItem('userClaims'));
    this.getTenants();
  }

  async getTenants() {
    let service = this.injector.get(MultiTenantUserAccessMapService);
    var response = await service.GetUserTenantRoleMap(this.userData.UserIdentifier);
    this.tenants = response.List;
  }

  async onTenantSelect(tenant: any) {
    debugger
    this.userData.Privileges = await this.getUserRoles(tenant.RoleId, tenant.TenantCode);
    if (this.roleDetail.IsActive == false) {
      this._messageDialogService.openDialogBox('Error', "User role is deactivated.", Constants.msgBoxError);
      this.localstorageservice.removeLocalStorageData();
    }
    else {
      if (this.userData.Privileges.length == 0 || this.userData.Privileges == null) {
        this._messageDialogService.openDialogBox('Error', "User role has no permission assigned", Constants.msgBoxError);
        this.localstorageservice.removeLocalStorageData();
      }
      else {
        localStorage.setItem('userClaims', JSON.stringify(this.userData));
        var userClaimsRolePrivilegeOperations = this.userData.Privileges;
        var isFound = false;
        var state = 0;
        this.statePrivilegeMap.forEach(map => {
          var isPresent = userClaimsRolePrivilegeOperations.filter(p => p.EntityName == map.Entity);
          if (isPresent != undefined && isPresent.length > 0) {
            if (isFound == false) {
              isFound = true;
              state = map.State;
            }
          }
        });
        if (isFound) {
          this.route.navigate([state]);
        }
      }
    }       
  }

  async getUserRoles(roleIdentifier, TenantCode) {
    let searchParameter: any = {};
    searchParameter.PagingParameter = {};
    searchParameter.PagingParameter.PageIndex = Constants.DefaultPageIndex;
    searchParameter.PagingParameter.PageSize = Constants.DefaultPageSize;
    searchParameter.SortParameter = {};
    searchParameter.SortParameter.SortColumn = Constants.Name;
    searchParameter.SortParameter.SortOrder = Constants.Ascending;
    searchParameter.SearchMode = Constants.Exact;
    searchParameter.IsRequiredRolePrivileges = true;
    searchParameter.Identifier = roleIdentifier;
    searchParameter.TenantCode = TenantCode;
    this.roleList = await this.loginService.getRoles(searchParameter);
    this.roleDetail = this.roleList[0];
    if (this.roleList.length > 0) {
      this.roleList.forEach(role => {
        role.RolePrivileges.forEach(privilege => {
          privilege.RolePrivilegeOperations.forEach(operation => {
            if (!this.rolePrivilegeExists(privilege.EntityName, operation.Operation)) {
              var obj = { EntityName: privilege.EntityName, Operation: operation.Operation }
              this.commonRolePrivileges.push(obj);
            }
          });
        });
      });
    }
    return this.commonRolePrivileges;
  }

  rolePrivilegeExists(entityName, operationName) {
    return this.commonRolePrivileges.some(function (el) {
      return (el.EntityName === entityName && operationName === el.Operation);
    });
  }

  onCancelTenantSelection() {
    this.localstorageservice.removeLocalStorageData();
    this.route.navigate(['login']);
  }

}
