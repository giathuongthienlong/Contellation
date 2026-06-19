# TextBox - Contellation Custom Controls

**Contellation TextBox** là control TextBox được mở rộng mạnh mẽ, hỗ trợ Placeholder, Icon, Clear Button và Masked Input.

---

## Tính năng nổi bật

- Placeholder linh hoạt (qua Attached Property)
- Hỗ trợ Icon bên trái/phải
- Nút Clear Button thông minh (chỉ hiện khi có nội dung và focus)
- Masked Input (hỗ trợ format số điện thoại, CMND, ngày tháng, mã số thuế...)
- Filter input (Number, Decimal...)
- Format mask prompt đẹp (hiển thị `__/__/____`)
- Đồng bộ giao diện với các control khác trong thư viện

## Dependency Properties

| Property             | Kiểu                      | Mặc định | Mô tả                              |
|----------------------|---------------------------|----------|------------------------------------|
| `ClearButtonEnabled` | `bool`                    | `true`   | Bật/tắt nút xóa                    |
| `ShowClearButton`    | `bool (ReadOnly)`         | `false`  | Trạng thái hiển thị nút xóa        |
| `Mask`               | `string`                  | `""`     | Chuỗi mask (ví dụ: (###) ###-####) |
| `PromptChar`         | `char`                    | `' '`    | Ký tự tạm thời trong mask          |
| `Filter`             | `TextBoxMaskedFilterType` | `Any`    | Bộ lọc ký tự đầu vào               |


## Dependency Properties

| Giá trị    | Mô tả                    |
|------------|--------------------------|
| `Any`      | `Cho phép tất cả`        |
| `Number`   | `Số nguyên (có dấu trừ)` |
| `UNumber`  | `Số nguyên dương`        |
| `Decimal`  | `Số thập phân`           |
| `UDecimal` | `Số thập phân dương`     |

---

## Cách sử dụng

### 1. Khai báo namespace

```xml
xmlns:ui="clr-namespace:Contellation.Custom.Controls;assembly=Contellation.Custom"
```

### 2. Ví dụ cơ bản

```xml
<ui:TextBox 
    Width="320" Height="42"
    ui:Element.PlaceholderText="Nhập họ và tên..."
    ui:Element.Icon="&#xE8BD;" 
    ui:Element.IconPlacement="Left"
    ClearButtonEnabled="True" />
```

### 3. Ví dụ Masked Input

```xml
<!-- Số điện thoại -->
<ui:TextBox Mask="(###) ### ####" PromptChar="_" />

<!-- CCCD / CMND -->
<ui:TextBox Mask="### ### ###" />

<!-- Ngày tháng năm -->
<ui:TextBox Mask="##/##/####" />

<!-- Chỉ cho phép số nguyên dương -->
<ui:TextBox Mask="" Filter="UNumber" />
```

### Best Practices
1. Nên dùng Attached Property el:Element.PlaceholderText và el:Element.Icon để đồng bộ giao diện.
2. Khi dùng Masked Input nên test kỹ chức năng xóa, paste, backspace.
3. Kết hợp với ResponsiveGrid để tạo form responsive đẹp.
4. Có thể tùy chỉnh MaskedPromptBrush để thay đổi màu prompt char.