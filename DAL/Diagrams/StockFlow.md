# Example Flows (Applied Scenarios)

This section illustrates how the above rules behave in real operational scenarios.

---

## 1. Stock is event-based

**Scenario:**
- System stock shows 10 PCS.
- User sells 2 PCS.

**Flow:**
1. Order created (no stock change)
2. StockForm (Fulfillment) posted
3. StockMovement created: `-2 PCS`

**Result:**
- Current stock = SUM(all movements)
- No direct stock update ever occurs

---

## 2. Base unit is immutable

**Scenario:**
- Product base unit = PIECE
- StockMovement exists (+100 PCS)

**Action:**
- User attempts to change base unit to BOX

**Result:**
- Operation is blocked
- User must create a new product variant instead

---

## 3. Units belong to documents, not stock

**Scenario:**
- Product: 1 BOX = 6 PCS
- User orders 2 BOX

**Flow:**
1. OrderLine stores: `Quantity = 2`, `Unit = BOX`
2. System calculates base quantity: `12 PCS`
3. StockReservation created for `12 PCS`
4. StockMovement created for `-12 PCS`

**Result:**
- User always sees BOX
- Stock always sees PCS

---

## 4. Separation of responsibilities (with quantity changes)

### Rule
- **Price changes never affect stock**
- **Quantity changes always affect reservations**
- **StockMovement is only affected on fulfillment**

---

### Scenario A: Order quantity increased (2 → 3 BOX)

**Tables involved:**
- `ORDERLINE`
- `STOCKRESERVATION`

**Flow:**
1. User updates `ORDERLINE.Quantity` from 2 → 3 (BOX)
2. System recalculates base quantity (12 → 18 PCS)
3. Update `STOCKRESERVATION.Quantity` from 12 → 18
4. No `STOCKMOVEMENT` created

**Result:**
- More stock is reserved
- Physical stock unchanged
- Order remains consistent

---

### Scenario B: Order quantity decreased (3 → 1 BOX)

**Tables involved:**
- `ORDERLINE`
- `STOCKRESERVATION`

**Flow:**
1. User updates `ORDERLINE.Quantity` from 3 → 1 (BOX)
2. System recalculates base quantity (18 → 6 PCS)
3. Update `STOCKRESERVATION.Quantity` from 18 → 6
4. No `STOCKMOVEMENT` created

**Result:**
- Reserved stock is released
- Stock availability increases
- No audit pollution

---

### Scenario C: Quantity changed after partial fulfillment

**Tables involved:**
- `ORDERLINE`
- `STOCKRESERVATION`
- `STOCKMOVEMENT`

**Flow:**
1. Original order: 3 BOX (18 PCS)
2. Fulfilled: 1 BOX (6 PCS) → `STOCKMOVEMENT` (-6)
3. User reduces order to 2 BOX (12 PCS)
4. System recalculates reservation:
   - New reservation = 12 - 6 = 6 PCS
5. Update `STOCKRESERVATION.Quantity` to 6

**Result:**
- Fulfilled stock remains consumed
- Remaining obligation adjusted
- History preserved

---

### Scenario D: Quantity reduced below fulfilled amount (invalid)

**Tables involved:**
- `ORDERLINE`
- `STOCKMOVEMENT`

**Flow:**
1. Fulfilled quantity = 12 PCS
2. User attempts to change order to 1 BOX (6 PCS)
3. System rejects the change

**Result:**
- Data integrity protected
- User must create return / credit flow

---

### Summary

| Change Type | ORDERLINE | STOCKRESERVATION | STOCKMOVEMENT |
|------------|----------|------------------|---------------|
| Price      | ✅ Update | ❌ No change     | ❌ No change  |
| Quantity ↑ | ✅ Update | 🔼 Increase      | ❌ No change  |
| Quantity ↓ | ✅ Update | 🔽 Decrease      | ❌ No change  |
| Fulfill    | ❌ No     | 🔽 Decrease      | ✅ Insert     |

---

### Hard rule

> If **ORDERLINE.Quantity ≠ (Fulfilled + Reserved)**  
> your system is broken.

---


## 5. StockReservation is a lock (not a suggestion)

### Rule
- `STOCKRESERVATION` represents **reserved but not yet consumed stock**
- Reserved stock **must not be available** to other orders
- Reservation quantity changes only when:
  - Order quantity changes
  - Fulfillment happens
  - Reservation is released or cancelled

---

### Scenario A: Partial fulfillment

**Tables involved:**
- `STOCKRESERVATION`
- `STOCKMOVEMENT`
- `STOCKFORM`
- `STOCKFORMLINE`

**Flow:**
1. Order placed for **10 PCS**
2. `STOCKRESERVATION` created:
   - `Quantity = 10`
   - `Status = Active`
3. Fulfillment process starts
4. `STOCKFORM` created (`FormType = Fulfillment`)
5. `STOCKFORMLINE.Quantity = 6`
6. `STOCKMOVEMENT` created:
   - `Quantity = -6`
   - `MovementType = Fulfillment`
7. `STOCKRESERVATION.Quantity` updated:
   - 10 → 4

**Result:**
- 6 PCS physically consumed
- 4 PCS remain locked
- No other order can access the remaining 4 PCS

---

### Scenario B: Attempt to reserve already reserved stock (blocked)

**Tables involved:**
- `STOCKRESERVATION`

**Flow:**
1. Available stock = 10 PCS
2. Existing reservation = 4 PCS
3. New order attempts to reserve 7 PCS
4. System calculates:
   - Available = 10 - 4 = 6
5. Reservation is rejected

**Result:**
- Overselling prevented
- Data remains consistent

---

### Scenario C: Reservation released (order cancelled)

**Tables involved:**
- `STOCKRESERVATION`

**Flow:**
1. Order cancelled
2. `STOCKRESERVATION.Status` set to `Released`
3. `STOCKRESERVATION.Quantity` set to `0`

**Result:**
- Stock unlocked
- Immediately available to other orders
- No `STOCKMOVEMENT` created

---

### Scenario D: Reservation expires automatically

**Tables involved:**
- `STOCKRESERVATION`

**Flow:**
1. Reservation created (`Status = Active`)
2. No fulfillment within allowed time
3. Background job marks reservation as `Released`

**Result:**
- Locked stock returned to availability
- Prevents dead stock

---

### Scenario E: Fulfillment consumes entire reservation

**Tables involved:**
- `STOCKRESERVATION`
- `STOCKMOVEMENT`

**Flow:**
1. Reservation = 10 PCS
2. Fulfillment = 10 PCS
3. `STOCKMOVEMENT` created (`-10`)
4. `STOCKRESERVATION.Quantity` → 0
5. `STOCKRESERVATION.Status` → `Consumed`

**Result:**
- Reservation fully closed
- Clean audit trail

---

### Scenario F: Multiple partial fulfillments

**Tables involved:**
- `STOCKRESERVATION`
- `STOCKMOVEMENT`

**Flow:**
1. Reservation = 10 PCS
2. Fulfillment #1 = 3 PCS
3. Reservation → 7 PCS
4. Fulfillment #2 = 2 PCS
5. Reservation → 5 PCS

**Result:**
- Incremental consumption supported
- Accurate remaining lock

---

### Summary

| Action | STOCKRESERVATION | STOCKMOVEMENT |
|------|------------------|---------------|
| Order placed | +Reserve | ❌ |
| Partial fulfill | 🔽 Reduce | ✅ |
| Cancel | Release | ❌ |
| Expire | Release | ❌ |
| Full fulfill | Consumed | ✅ |

---

### Hard rules

- Reserved stock **must be excluded** from availability calculations
- `STOCKMOVEMENT` **never creates availability**, it only consumes or adds physical stock
- If reservations are ignored, **overselling is guaranteed**

---



## 6. StockMovement is truth

**Scenario:**
- Stock count discrepancy discovered

**Flow:**
1. Adjustment StockForm created
2. Difference calculated (e.g. -3 PCS)
3. StockMovement added: `-3 PCS`

**Result:**
- Historical movements preserved
- Adjustment is auditable

---

## 7. StockForm represents intent

**Scenario:**
- Warehouse prepares a transfer

**Flow:**
1. StockForm created (Draft)
2. No stock change
3. StockForm approved
4. StockMovements generated

**Result:**
- Planning and execution are separated

---

## 8. StockFormLine stores human input

**Scenario:**
- User counts stock as 5 BOX

**Flow:**
1. StockFormLine saved: `5 BOX`
2. Conversion at posting: `30 PCS`
3. StockMovement recorded: `+30 PCS`

**Result:**
- Document remains human-readable
- Stock remains consistent

---

## 9. Partial fulfillment

**Scenario:**
- Order requires 10 PCS
- Warehouse ships 4 PCS today

**Flow:**
1. StockMovement: `-4 PCS`
2. Reservation reduced to 6 PCS
3. Order remains partially open

**Result:**
- Multiple fulfillments supported

---

## 10. Cancellation vs Return

**Scenario A (Cancellation):**
- Order cancelled before shipping

**Flow:**
1. Reservation released
2. No StockMovement created

**Scenario B (Return):**
- Order shipped and later returned

**Flow:**
1. StockMovement: `+X PCS` (Return)

**Result:**
- Cancellation ≠ Return

---

## 11. Stock transfer

**Scenario:**
- Move 20 PCS from Warehouse A to B

**Flow:**
1. StockMovement A: `-20 PCS`
2. StockMovement B: `+20 PCS`

**Result:**
- No implicit movement
- Full traceability

---

## 12. Warehouse scope

**Scenario:**
- Two shops share central warehouse

**Flow:**
- All reservations and movements reference the same warehouse
- Shop context is handled at order level

**Result:**
- Clean stock ownership

---

## 13. Frozen unit conversion

**Scenario:**
- BOX multiplier changes from 6 → 8

**Flow:**
- Old StockMovements remain unchanged
- New documents use new multiplier

**Result:**
- Historical correctness preserved

---

## 14. Base unit enforcement

**Scenario:**
- User creates product

**Flow:**
1. Base unit selected (PIECE)
2. Product saved
3. Base unit becomes read-only

**Result:**
- Incorrect future edits prevented

---

## 15. New variant for semantic change

**Scenario:**
- Product now sold only in cartons

**Flow:**
1. New ProductVariant created
2. Stock transferred via StockForm
3. Old variant remains historical

**Result:**
- No data corruption

---

## 16. No direct stock manipulation

**Scenario:**
- Developer attempts to insert StockMovement manually

**Result:**
- Operation rejected by domain logic / DB rules

---

## 17. Audit-first design

**Scenario:**
- External audit requests stock history

**Flow:**
- All changes traced via StockMovement
- No recalculation or overwrite needed

**Result:**
- Audit passes

---

## 18. Future complexity readiness

**Scenario:**
- Business adds service requests + partial stock usage

**Flow:**
- ServiceRequest creates reservation
- Fulfillment consumes reservation
- Returns handled independently

**Result:**
- No schema change required

---
