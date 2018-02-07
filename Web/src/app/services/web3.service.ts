import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import Web3 from 'web3';
import { EventsService } from 'angular-event-service';

declare let window: any;

@Injectable()
export class Web3Service {

  private web3: any;
  public account: string;
  public network: string;

  constructor(private router: Router, private eventsService: EventsService) {
    this.initProvider();
  }

  initProvider() {
    let web3Service = this;

    window.addEventListener('load', function () {
      if (typeof window.web3 !== 'undefined') {
        web3Service.web3 = new Web3(window.web3.currentProvider);
        // web3Service.checkNetwork();
        // web3Service.checkAccount();
        // web3Service.checkAUCBalance();
      }
      else {
        // web3Service.eventsService.broadcast('networkCheck', -1);
        // web3Service.eventsService.broadcast('accountChange');
        // web3Service.redirect();
      }
      web3Service.eventsService.broadcast('web3Loaded', window.web3);
    })
  }

  checkAccount() {
    let web3Service = this;
    this.account = this.web3.eth.accounts[0];
    this.eventsService.broadcast('accountChange', this.account);
    var accountInterval = setInterval(function () {
      this.web3.eth.getAccounts(function (err, accounts) {
        var currentAccount = accounts.length > 0 ? accounts[0] : null;
        if (web3Service.account != currentAccount){
          web3Service.account = currentAccount;
          web3Service.eventsService.broadcast('accountChange', currentAccount);
        }
      });
    }, 100);
  }

  // redirect() {
  //   if (this.router.url != "/required") {
  //     this.router.navigate(['required'])
  //   }
  // }

  checkNetwork() {
    let web3Service = this;
    this.web3.version.getNetwork((err, netId) => {
      web3Service.network = netId;
      this.eventsService.broadcast('networkCheck', netId);
      // switch (netId) {
      //   case "1":
      //     web3Service.redirect();
      //     break
      //   case "2":
      //     web3Service.redirect();
      //     break
      //   case "3":
      //     web3Service.redirect();
      //     break
      //   case "4":
      //     console.log('This is the Rinkeby test network.')
      //     break
      //   case "42":
      //     web3Service.redirect();
      //     break
      //   default:
      //     web3Service.redirect();
      // }
    })
  }

  public getTokenBalance(tknContractAddress) {
    let web3Service = this;
    this.web3.eth.call({
      to: tknContractAddress, // Contract address, used call the token balance of the address in question
      data: '0x70a08231000000000000000000000000' + web3Service.account // Combination of contractData and tknAddress, required to call the balance of an address 
    }, function (err, result) {
      if (result) {
        var tokens = web3Service.web3.utils.toBN(result).toString(); // Convert the result to a usable number string
        console.log('AUCT Tokens Owned: ' + web3Service.web3.utils.fromWei(tokens, 'ether')); // Change the string to be in Ether not Wei, and show it in the console
      }
      else {
        console.log(err); // Dump errors here
      }
    });
  }
}
