import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import Web3 from 'web3';
import { EventsService } from 'angular-event-service';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { constants } from '../util/contants';

declare let window: any;

@Injectable()
export class Web3Service {

  private web3: Web3;

  constructor(private router: Router, private eventsService: EventsService) {
  }

  public getWeb3(): Observable<Web3> {
    let self = this;
    return new Observable(observer => {
      if (self.web3) {
        observer.next(self.web3);
      }
      else {
        window.addEventListener('load', function () {
          if (typeof window.web3 !== 'undefined') {
            self.web3 = new Web3(window.web3.currentProvider);
            observer.next(self.web3);
          }
          else {
            observer.next(null);
          }
        })
      }
    });
  }

  public getNetwork(): Observable<number> {
    let self = this;
    return new Observable(observer => {
      self.web3.eth.net.getId((err, netId) => {
        observer.next(netId);
      });
    })
  }

  public getAccount(): Observable<string> {
    let self = this;
    return new Observable(observer => {
      if (!self.web3) {
        observer.next(null);
      }
      else {
        self.web3.eth.getAccounts(function (err, accounts) {
          var currentAccount = accounts.length > 0 ? accounts[0] : null;
          observer.next(currentAccount);
        });
      }
    });
  }

  public getTokenBalance(tknContractAddress: string, accountAddrs: string): Observable<number> {
    let self = this;
    return new Observable(observer => {
      self.web3.eth.call({
        to: tknContractAddress, // Contract address, used call the token balance of the address in question
        data: '0x70a08231000000000000000000000000' + (accountAddrs).substring(2) // Combination of contractData and tknAddress, required to call the balance of an address 
      }).then(function (result) {
        if (result) {
          var tokens = self.web3.utils.toBN(result).toString(); // Convert the result to a usable number string
          var tokensEther = self.web3.utils.fromWei(tokens, 'ether');
          observer.next(parseFloat(tokensEther));
        }
        else {
          observer.next(0);
        }
      });
    });
  }

}
