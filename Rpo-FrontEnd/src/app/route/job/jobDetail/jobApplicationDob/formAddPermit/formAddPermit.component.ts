import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { cloneDeep, identity, pickBy, intersectionBy } from 'lodash';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { JobPermit } from '../../../../../types/jobPermit';
import { JobApplicationService } from '../../../../../services/JobApplicationService.services';
import { Message } from "../../../../../app.messages";
import * as moment from 'moment';
import { TaskServices } from '../../../../task/task.services';
import { constantValues } from '../../../../../app.constantValues';
import * as _ from 'underscore';

declare const $: any
/**
* This component contains all function that are used in FormAddPermit
* @class FormAddPermit
*/
@Component({
  selector: '[form-add-permit]',
  templateUrl: './formAddPermit.component.html'
})
export class FormAddPermit {
  @Input() modalRef: BsModalRef
  @Input() onSave: Function
  @Input() idApplicationNumber: any
  @Input() formAddPermit: JobPermit
  @Input() appType: any
  @Input() reload: Function
  @Input() idJob: number
  @Input() idJobAppObject: any
  @Input() isNew: boolean
  @Input() idApp: any
  @Input() isDep: any

  private selectedJobType: number
  private jobPermit: any
  private workPermit: any
  private errorMsg: {}
  private responsiblityList: any = []
  private jobContacts: any = []
  private loading: boolean = false
  private workPermitId: number
  private disablePerson: boolean = false;
  private selectedRow: any
  private showPGL: boolean = false
  /**
    * This method define all services that requires in whole class
    * @method constructor
  */
  constructor(
    private toastr: ToastrService,
    private message: Message,
    private JobApplicationService: JobApplicationService,
    private constantValues: constantValues,
    private taskServices: TaskServices

  ) {
    this.errorMsg = this.message.msg
  }

  /**
    * This method will be called once only when module is call for first time
    * @method ngOnInit
  */
  ngOnInit() {
    this.jobPermit = {} as JobPermit
    this.loading = true
    if (this.appType.idJobApplicationType) {
      this.JobApplicationService.getResponsibleDropDown().subscribe(r => {
        this.responsiblityList = r
      }, e => { })
      this.JobApplicationService.getWorkPermitTypes(this.appType.idJobApplicationType).subscribe(r => {
        this.workPermit = r
        this.jobPermit.idJob = this.idJob
        this.contacts(this.idJob)
        this.jobPermit.idJobApplication = this.idApp
        this.jobPermit.idApplicationNumber = this.appType.applicationNumber
        this.jobPermit.applicationType = this.appType.idJobApplicationType
        this.jobPermit.jobApplicationTypeName = this.appType.jobApplicationTypeName
        this.jobPermit.applicationNumber = this.idApplicationNumber
        if (this.appType.id && this.isNew) {
          this.workPermitId = this.appType.id
          this.getWorkPermit()
        } else {
          this.loading = false
        }
      }, e => { this.loading = false })
    } else {
      this.loading = false
    }
  }

  /**
   * This method disable person responsible
   * @method disablePersonResposible
   */
  disablePersonResposible() {
    if (this.jobPermit.idResponsibility == 1) {
      this.jobPermit.idContactResponsible = null;
      this.jobPermit.companyResponsible = "";
      this.disablePerson = true;
    } else {
      this.disablePerson = false;
    }

    if (this.jobPermit.idResponsibility == null) {
      this.jobPermit.idContactResponsible = null;
      this.jobPermit.companyResponsible = "";
    }
  }

  /**
   * This method get contacts list
   * @method contacts
   * @param {number} idJob ID of Job 
   */
  contacts(idJob: number) {
    this.JobApplicationService.getJobContacts(idJob).subscribe(r => {
      if (r.data.length > 0) {
        let data = r.data
        let contacts = _.sortBy(data, function (data: any) { return data.contactName.toLowerCase(); });
        this.jobContacts = contacts
      }
    }, e => { this.loading = false })
  }

  /**
   * This method will call when work permit type changed
   * @method onChangeWorkPermitType
   * @param {any} event Event object
   */
  onChangeWorkPermitType(event: any) {
    this.showPGL = false
    this.jobPermit.code = ""
    this.jobPermit.workDescription = ""
    if (event && event.code) {
      this.jobPermit.code = event.code
      if (this.jobPermit.code == 'Demolition' || this.jobPermit.code == 'OT/GC' ||
        this.jobPermit.code == 'OT/FO' || this.jobPermit.code == 'OT/EA' ||
        this.jobPermit.code == 'OT' || this.jobPermit.code == 'NB') {
        this.showPGL = true
      }
    }
    if (event && event.content) {
      this.jobPermit.workDescription = event.content
    }
  }

  /**
   * This method will get Work Permit list
   * @method getWorkPermit
   */
  getWorkPermit() {
    this.JobApplicationService.getWorkPermitById(this.idJobAppObject).subscribe(r => {
      this.jobPermit = {} as JobPermit
      this.jobPermit = r
      this.jobPermit.idJob = this.idJob
      this.jobPermit.idJobApplication = this.idApp
      this.jobPermit.idApplicationNumber = this.appType.applicationNumber
      this.jobPermit.applicationNumber = this.idApplicationNumber
      this.jobPermit.applicationType = this.appType.idJobApplicationType
      this.jobPermit.jobApplicationTypeName = this.appType.jobApplicationTypeName
      if (this.jobPermit.expires) {
        this.jobPermit.expires = this.taskServices.dateFromUTC(this.jobPermit.expires, true);
      }
      if (this.jobPermit.filed) {
        this.jobPermit.filed = this.taskServices.dateFromUTC(this.jobPermit.filed, true);
      }
      if (this.jobPermit.issued) {
        this.jobPermit.issued = this.taskServices.dateFromUTC(this.jobPermit.issued, true);
      }
      if (this.jobPermit.signedOff) {
        this.jobPermit.signedOff = this.taskServices.dateFromUTC(this.jobPermit.signedOff, true);
      }
      if (this.jobPermit.withdrawn) {
        this.jobPermit.withdrawn = this.taskServices.dateFromUTC(this.jobPermit.withdrawn, true);
      }
      if (this.jobPermit.plumbingSignedOff) {
        this.jobPermit.plumbingSignedOff = this.taskServices.dateFromUTC(this.jobPermit.plumbingSignedOff, true);
      }
      if (this.jobPermit.constructionSignedOff) {
        this.jobPermit.constructionSignedOff = this.taskServices.dateFromUTC(this.jobPermit.constructionSignedOff, true);
      }
      if (this.jobPermit.finalElevator) {
        this.jobPermit.finalElevator = this.taskServices.dateFromUTC(this.jobPermit.finalElevator, true);
      }
      if (this.jobPermit.tempElevator) {
        this.jobPermit.tempElevator = this.taskServices.dateFromUTC(this.jobPermit.tempElevator, true);
      }
      if (this.jobPermit && this.jobPermit.code) {
        if (this.jobPermit.code == 'Demolition' || this.jobPermit.code == 'OT/GC' ||
          this.jobPermit.code == 'OT/FO' || this.jobPermit.code == 'OT/EA' ||
          this.jobPermit.code == 'OT' || this.jobPermit.code == 'NB') {
          this.showPGL = true
        }
      }
      this.disablePersonResposible()
      this.loading = false

    }, e => { this.loading = false })
  }

  /**
   * This method will save permit
   * @method savePermit
   */
  savePermit() {
    this.loading = true
    let newApplication = false
    if (this.jobPermit.id && this.jobPermit.id > 0) {
      newApplication = false
    } else {
      newApplication = true
    }
    this.jobPermit.filed = $('#fileddate').val();
    this.jobPermit.issued = $('#issueddate').val();
    this.jobPermit.expires = $('#expiresdate').val();
    this.jobPermit.signedOff = $('#signOffdate').val();
    this.jobPermit.withdrawn = $('#withdrawn').val();
    this.jobPermit.plumbingSignedOff = $('#plumbingSignedOff').val();
    this.jobPermit.constructionSignedOff = $('#constructionSignedOff').val();
    this.jobPermit.finalElevator = $('#finalElevator').val();
    this.jobPermit.tempElevator = $('#tempElevator').val();
    this.jobPermit.permitNumber = $('#permitNumber').val();
    delete this.jobPermit.lastModifiedDate
    if (this.jobPermit.code != 'TCO') {
      delete this.jobPermit.plumbingSignedOff
      delete this.jobPermit.constructionSignedOff
      delete this.jobPermit.finalElevator
      delete this.jobPermit.tempElevator
    }
    this.loading = true
    this.jobPermit.companyResponsible = null
    this.JobApplicationService.addEditWorkPermit(this.jobPermit, newApplication).subscribe(r => {
      if (newApplication) {
        this.toastr.success('Work Permit added successfully')
      } else {
        this.toastr.success('Application updated successfully')
      }
      this.reload()
      this.modalRef.hide()

      this.loading = false
    }, e => {
      this.modalRef.hide()
      this.loading = false
    })
  }

  /**
   * This method will get company list
   * @method getCompany
   * @param {any} e company object 
   */
  getCompany(e: any) {
    if (e != null) {
      if (e.companyName) {
        this.jobPermit.companyResponsible = e.companyName
      } else {
        this.jobPermit.companyResponsible = 'Individual'
      }
    } else {
      this.jobPermit.companyResponsible = ""
    }
  }

  /**
   * This method will check given number is decimal or not
   * @method isDecimal
   * @param {any} evt Event Object 
   */
  isDecimal(evt: any) {
    //getting key code of pressed key
    var keycode = (evt.which) ? evt.which : evt.keyCode;
    //comparing pressed keycodes
    if (!(keycode == 8 || keycode == 46) && (keycode < 48 || keycode > 57)) {
      return false;
    }
    else {
      var parts = evt.srcElement.value.split('.');
      if (parts.length > 1 && keycode == 46)
        return false;
      return true;
    }
  }

  /**
   * This method converts string date into date object
   * @method getTheDateObject
   * @param {any} date String Date 
   */
  getTheDateObject(date: any) {
    return new Date(date)
  }

}