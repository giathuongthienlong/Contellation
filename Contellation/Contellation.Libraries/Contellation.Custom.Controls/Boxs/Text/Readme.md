# TextBox - Contellation Custom Controls

**Contellation TextBox** là control TextBox được mở rộng mạnh mẽ, hỗ trợ Placeholder, Icon, Clear Button và Masked Input.

---

## Tính năng nổi bật

- Placeholder linh hoạt (qua Attached Property)
- Hỗ trợ Icon bên trái/phải
- Nút Clear Button tự động hiện khi có nội dung và đang focus
- Masked Input (hỗ trợ format số điện thoại, CMND, ngày tháng, mã số thuế...)
- Filter input (Number, Decimal, Any...)
- Dễ dàng tùy chỉnh giao diện

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
Nên dùng Attached Property từ ui:Element để đặt Placeholder và Icon (để đồng bộ với các control khác).
Khi dùng Masked Input nên test kỹ chức năng xóa, paste, backspace.
Kết hợp với ResponsiveGrid để tạo form đẹp và responsive.
Có thể mở rộng thêm Appearance (Outline, Filled...) sau.