import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import Web3 from 'web3';
import Web3Utils from 'web3-utils';
import { EventsService } from 'angular-event-service';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { constants } from '../util/constants';

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
      self.web3.version.getNetwork((err, netId) => {
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
      }, function (err, result) {
        if (result) {
          var tokens = Web3Utils.toBN(result).toString(); // Convert the result to a usable number string
          var tokensEther = Web3Utils.fromWei(tokens, 'ether');
          observer.next(parseFloat(tokensEther));
        }
        else {
          observer.next(0);
        }
      });
    });
  }

  public toHex(val: string): string {
    return Web3Utils.toHex(val);
  }

  public toWei(value: string, unit?: string) {
    return Web3Utils.toWei(value, unit);
  }

  public getContractMethodData(abi: string, contractAddress: string, method: string, ...params: any[]) {
    var contractInstance = this.web3.eth.contract(JSON.parse(abi)).at(contractAddress);
    var data = contractInstance[method].getData.apply(null, params);
    return data;
  }

  public sendTransaction(gasPrice: number, gasLimit: number, from: string, to: string,
    value: number, data: string, chainId: string): Observable<string> {

    const gasPriceWei = Web3Utils.toWei(gasPrice.toString(), 'gwei');
    const valueWei = Web3Utils.toWei(value.toString(), 'ether');

    let self = this;
    return new Observable(observer => {

      this.web3.eth.getTransactionCount(from,
        function (err, result) {
          if (err) observer.next(null);
          else {
            var transactionObj = {
              nonce: Web3Utils.toHex(result),
              gasPrice: Web3Utils.toHex(gasPriceWei),
              gasLimit: Web3Utils.toHex(gasLimit),
              from: from,
              to: to,
              value: Web3Utils.toHex(valueWei),
              data: data,
              chainId: Web3Utils.toHex(chainId)
            };

            self.web3.eth.sendTransaction(transactionObj,
              function (err, result) {
                if (result) {
                  observer.next(result);
                }
                else {
                  observer.next(null);
                }
              });
          }
        })


    });
  }

  public isTransactionMined(hash) : Observable<boolean> {
    return new Observable(observer => this.web3.eth.getTransactionReceipt(hash, function(err, receipt){ 
      observer.next(receipt && receipt.blockNumber);
    }));
  }
}
