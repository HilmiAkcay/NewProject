# Purchase Flow – Purchase Order → Supplier Invoice

## Scenario
The company purchases **Coca-Cola 330ml** from a supplier and processes a purchase order, goods receipt, and supplier invoice.

---

## 1. Product & Unit Structure

The product is already defined in the system.

- **Product:** Coca-Cola  
- **Variant:** Coca-Cola 330ml  
- **Purchase Unit:** Box (24 pieces)  
- **Base Unit:** Piece  

The unit multiplier ensures correct stock conversion.

---

## 2. Purchase Unit Rules (Supplier-Specific)

Purchase constraints are defined per supplier.

**Purchase Unit Rule:**
- **Supplier:** Supplier A  
- **Product Unit:** Box (24 pcs)  
- **Min Order Quantity:** 5  
- **Max Order Quantity:** 100  
- **Order Multiple:** 5  
- **Allow Partial:** false  

These rules control how purchase orders can be created.

---

## 3. Purchase Price Definition

A purchase price is defined separately from sales pricing.

- **Product Unit:** Box (24 pcs)  
- **Supplier:** Supplier A  
- **Net Price:** 180.00  
- **Currency:** EUR  
- **Tax Rate:** VAT 21%  
- **Validity:** Active for the current date  

Purchase prices always represent supplier costs.

---

## 4. Purchase Order Creation Flow

### Step 1 – Product Selection
- The buyer selects the product and purchase unit.
- The system identifies the correct supplier rules.

---

### Step 2 – Rule Validation
The system validates the entered quantity:

- Quantity must be ≥ minimum order quantity  
- Quantity must be ≤ maximum order quantity  
- Quantity must match the defined order multiple  
- Partial quantities are blocked if not allowed  

Invalid quantities are rejected before the order is created.

---

### Step 3 – Price Resolution
- The system retrieves the active purchase price for the supplier.
- Validity dates are checked automatically.

---

### Step 4 – Purchase Order Calculation
Based on the purchase price:

- **Net Total:** Quantity × Net Price  
- **Tax Amount:** Calculated using the assigned tax rate  
- **Gross Total:** Net + Tax  

Purchase calculations prioritize accounting accuracy.

---

## 5. Goods Receipt (Inventory Impact)

When goods are received:

- Ordered boxes are converted into base units  
- Stock is updated using the unit multiplier  

**Example:**

This guarantees consistent stock tracking.

---

## 6. Supplier Invoice Processing

### Step 1 – Invoice Matching
The supplier invoice is matched against:
- Purchase order quantity
- Agreed purchase price
- Tax rate

---

### Step 2 – Variance Detection
Any mismatch in:
- Quantity
- Price
- Tax  

is detected immediately and flagged for review.

---

### Step 3 – Invoice Posting
- The supplier invoice is approved
- Accounting entries are generated
- The purchase cycle is closed

---

## 7. Business Benefits

- Enforces supplier-specific purchase constraints
- Prevents incorrect order quantities
- Keeps purchase costs separate from sales prices
- Ensures accurate inventory valuation
- Simplifies invoice matching and auditing

---

## Notes

This purchase flow supports:
- Multiple suppliers per product
- Different packaging per supplier
- Supplier-specific pricing and rules
- Scalable procurement processes

The structure is designed for long-term ERP stability.

