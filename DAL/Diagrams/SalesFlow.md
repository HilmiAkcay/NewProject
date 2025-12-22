# Sales Flow – Invoice Creation

## Scenario
A store sells **Coca-Cola 330ml** to a customer and issues an invoice.

---

## 1. Product Structure

The product is defined once and reused across sales, inventory, and pricing.

- **Product:** Coca-Cola  
- **Variant:** Coca-Cola 330ml  
- **Sales Unit:** Piece  
- **Barcode:** 5449000000996  

This structure ensures the same product can be priced, stocked, and sold consistently.

---

## 2. Sales Price Definition

A sales price is configured in advance.

- **Product Unit:** Piece  
- **Price Amount:** 10.00  
- **Currency:** EUR  
- **Tax Rate:** VAT 21%  
- **Calculation Policy:** Retail Policy  
- **Validity:** Active for the current date  

The sales price defines what the customer pays and how tax is calculated.

---

## 3. Calculation Policy

Each sales price references a calculation policy.

**Retail Policy settings:**
- **Price Type:** Gross (tax included)  
- **Tax Calculation Level:** Line  
- **Line Rounding Rule:** Half-up (2 decimals)  
- **Receipt Rounding Rule:** Cash rounding to 0.05  

This guarantees consistent tax and rounding behavior across all invoices.

---

## 4. Invoice Creation Flow

### Step 1 – Product Identification
- The cashier scans the barcode or selects the product.
- The system resolves the correct product unit.

---

### Step 2 – Price Resolution
- The system finds the active sales price for the product unit.
- Validity dates are checked automatically.

---

### Step 3 – Line Calculation
Based on the calculation policy:

- **Unit Price (Gross):** 10.00  
- **VAT (21%):** 1.74  
- **Net Amount:** 8.26  

Line-level rounding is applied according to the policy.

---

### Step 4 – Receipt Calculation
- All invoice lines are summed.
- Receipt-level rounding is applied if required.

---

### Step 5 – Invoice Finalization
- Invoice totals are calculated.
- Tax values are stored correctly.
- The invoice is printed or sent digitally.

---

## 5. Final Invoice Result

- The customer sees a clear final price.
- Tax amounts are consistent and traceable.
- Accounting data is accurate and compliant.

---

## 6. Business Benefits

- Prevents inconsistencies between tax-included and tax-excluded amounts  
- Ensures identical calculations across POS, web, and B2B channels  
- Separates pricing logic from product definition  
- Supports future tax or rounding rule changes without data restructuring  

---

## Notes

This sales flow applies equally to:
- Retail POS
- Online sales
- B2B invoicing  

Only the **sales price** or **calculation policy** changes — the core structure remains stable.
