import { AdvisorPurchaseInfo } from "./advisorPurchaseInfo";
import { AdvisorHistory } from "./advisorHistory";

export class AdvisorDetail {
  purchaseInfo: AdvisorPurchaseInfo;
  advisorId: number;
  date: Date;
  description: number;
  price: number;
  period: number;
  enabled: boolean;
  portfolioHistory: AdvisorHistory[]
}
