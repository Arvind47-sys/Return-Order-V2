import { ProcessingChargeDetails } from "./processing-charge.model";

export interface PaymentInfo {
    requestId: number;
    creditCardNumber: number;
    creditLimit: number;
    totalProcessingCharge: number;
    processResponse: ProcessingChargeDetails;
}