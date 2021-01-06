import { ChartComponent } from "./ChartComponent-a7f89f69";
declare class DateTimePicker extends ChartComponent {
    private calendar;
    private calendarPicker;
    private timeControls;
    private minMillis;
    private maxMillis;
    private fromMillis;
    private toMillis;
    private fromMinutes;
    private fromHours;
    private toMinutes;
    private toHours;
    private onSet;
    private onCancel;
    private isValid;
    private targetElement;
    private dateTimeSelectionPanel;
    private quickTimesPanel;
    private isSettingStartTime;
    private startRange;
    private endRange;
    private anchorDate;
    private offsetName;
    private fromInput;
    private toInput;
    private quickTimeArray;
    constructor(renderTarget: Element);
    // returns -1 if not currently a quicktime
    private getCurrentQuickTime;
    getQuickTimeText(quickTimeMillis: any): any;
    private convertToCalendarDate;
    private setNewOffset;
    private onSaveOrCancel;
    render(chartOptions: any, minMillis: number, maxMillis: number, fromMillis?: number, toMillis?: number, onSet?: any, onCancel?: any): void;
    private updateDisplayedDateTimes;
    private setFromQuickTimes;
    private buildQuickTimesPanel;
    private createTimeString;
    private getTimeFormat;
    updateFromAndTo(fromMillis: any, toMillis: any): void;
    private createTimezonePicker;
    //zero out everything but year, month and day
    private roundDay;
    private setTimeRange;
    private createCalendar;
    private setSelectedQuickTimes;
    private setFromDate;
    private setToDate;
    private setIsSaveable;
    //line up the seconds and millis with the second and millis of the max date
    private adjustSecondsAndMillis;
    private setFromMillis;
    private setToMillis;
    private displayRangeErrors;
    private rangeIsValid;
    private updateDisplayedFromDateTime;
    private updateDisplayedToDateTime;
    private offsetUTC;
    private offsetFromUTC;
    private checkDateTimeValidity;
    private setTimeInputBox;
    private createTimePicker;
}
export { DateTimePicker as default };