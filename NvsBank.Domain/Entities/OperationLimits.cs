namespace NvsBank.Domain.Entities;

public class OperationLimits
{
    // --- Customer ---
    public decimal CustomerSingleTransferLimit { get; set; }
    public decimal CustomerDailyTransferLimit { get; set; }
    public decimal CustomerTotalTransferredToday { get; set; }

    public decimal CustomerSinglePaymentLimit { get; set; }
    public decimal CustomerDailyPaymentLimit { get; set; }
    public decimal CustomerTotalPaidToday { get; set; }

    // --- Teller ---
    public decimal TellerSingleWithdrawalLimit { get; set; }
    public decimal TellerDailyWithdrawalLimit { get; set; }
    public decimal TellerTotalWithdrawnToday { get; set; }

    public decimal TellerSingleDepositLimit { get; set; }
    public decimal TellerDailyDepositLimit { get; set; }
    public decimal TellerTotalDepositedToday { get; set; }

    // --- Analyst ---
    public decimal ApprovalLimit { get; set; }
    public int MaxPendingApprovals { get; set; }
    public int CurrentPendingApprovals { get; set; }

    // --- Admin ---
    public bool CanOverrideLimits { get; set; } = true;

    public bool CanCustomerTransfer(decimal amount)
    {
        if (amount > CustomerSingleTransferLimit)
            return false;

        if (CustomerTotalTransferredToday + amount > CustomerDailyTransferLimit)
            return false;

        return true;
    }

    public bool CanCustomerPay(decimal amount)
    {
        if (amount > CustomerSinglePaymentLimit)
            return false;

        if (CustomerTotalPaidToday + amount > CustomerDailyPaymentLimit)
            return false;

        return true;
    }

    public bool CanTellerWithdraw(decimal amount)
    {
        if (amount > TellerSingleWithdrawalLimit)
            return false;

        if (TellerTotalWithdrawnToday + amount > TellerDailyWithdrawalLimit)
            return false;

        return true;
    }

    public bool CanTellerDeposit(decimal amount)
    {
        if (amount > TellerSingleDepositLimit)
            return false;

        if (TellerTotalDepositedToday + amount > TellerDailyDepositLimit)
            return false;

        return true;
    }

    public bool CanAnalystApprove(decimal amount)
    {
        if (amount > ApprovalLimit)
            return false;

        if (CurrentPendingApprovals >= MaxPendingApprovals)
            return false;

        return true;
    }

    public bool CanAdminOverride()
    {
        return CanOverrideLimits;
    }
}