import React, { useEffect, useRef } from 'react';
import moment from 'moment';
import { DatePicker, FormField, FormFieldHelperText } from 'your-component-library'; // Update with actual imports
import { Constants } from 'your-constants-file'; // Update with actual path

const DatePickerControl = (props: IDatePickerProps) => {
  const classes = localStyles();
  const startYear = moment().subtract(10, 'years');
  const endYear = moment().add(5, 'years');
  const customYearDropdownRange: YearOption[] = [];

  for (let year = startYear.year(); year <= endYear.year(); year++) {
    customYearDropdownRange.push({ value: year, text: year.toString() });
  }

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
    },
  } : {};

  // Ref for the DatePicker
  const datePickerRef = useRef<HTMLDivElement | null>(null);

  useEffect(() => {
    const resizeHandler = () => {
      window.dispatchEvent(new Event('resize'));
    };

    // Trigger the resize handler after the component mounts
    resizeHandler();

    return () => {
      // Cleanup if needed
    };
  }, []);

  return (
    <FormField validationStatus={props.validationState}>
      <div ref={datePickerRef}>
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
      </div>
      <FormFieldHelperText>
        {props.validationState === 'error' ? props.validationText : ''}
      </FormFieldHelperText>
    </FormField>
  );
};

export default DatePickerControl;
