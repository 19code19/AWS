const DatePickerControl = (props: IDatePickerProps) => {
  const classes = localStyles();
  const startYear = moment().subtract(10, 'years');
  const endYear = moment().add(5, 'years');
  const customYearDropdownRange: YearOption[] = [];

  for (let year = startYear.year(); year <= endYear.year(); year++) {
    customYearDropdownRange.push({ value: year, text: year.toString() });
  }

  // Popper Props with proper placement and higher z-index
  const popperProps = props.openAtTop ? {
    placement: 'top-start',
    popperOptions: {
      modifiers: [
        {
          name: 'zIndex',
          options: {
            zIndex: 1000, // Ensure DatePicker is above other elements
          },
        },
      ],
      onCreate: (state: any) => {
        setTimeout(() => {
          window.dispatchEvent(new Event('resize'));
        }, 1);
      },
    },
  } : {};

  return (
    <FormField validationStatus={props.validationState}>
      <DatePicker
        PopperProps={popperProps} // Apply the modified popperProps
        InputProps={{
          placeholder: 'Select Date',
          onBlur: () => props.onChange(),
          classes: { mediumDensity: classes.datepicker },
        }}
        onInputValueChange={props.onChange}
        onDateChange={props.onChange}
        CalendarProps={{
          isDayBlocked: props.disableDate,
        }}
        YearDropdownProps={{
          source: customYearDropdownRange,
        }}
        InputValue={props.value}
        readOnly={props.disabled}
        dateFormat={Constants.datePickerFormat}
      />
      <FormFieldHelperText>
        {props.validationState === 'error' ? props.validationText : ''}
      </FormFieldHelperText>
    </FormField>
  );
};

export default DatePickerControl;
